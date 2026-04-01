using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;
using System.Net.Mail; //para aplicar la validacion de email.

namespace SportsLeague.Domain.Services
{
    public class SponsorService : ISponsorService
    {
        private readonly ISponsorRepository _sponsorRepository;
        private readonly ITournamentSponsorRepository _tournamentSponsorRepository;
        private readonly ITournamentRepository _tournamentRepository; 
        private readonly ILogger<SponsorService> _logger;  

        public SponsorService(
            ISponsorRepository sponsorRepository,
            ITournamentSponsorRepository tournamentSponsorRepository,
            ITournamentRepository tournamentRepository,
            ILogger<SponsorService> logger)
        {
            _sponsorRepository = sponsorRepository;
            _tournamentSponsorRepository = tournamentSponsorRepository;
            _tournamentRepository = tournamentRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Sponsor>> GetAllAsync()
        {
            _logger.LogInformation("Retrieving all sponsors");
            return await _sponsorRepository.GetAllAsync();
        }

        public async Task<Sponsor?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving sponsor with ID: {SponsorId}", id);
            var sponsor = await _sponsorRepository.GetByIdAsync(id);
            if (sponsor == null)
                _logger.LogWarning("Sponsor with ID {SponsorId} not found", id);
            return sponsor;
        }

        public async Task<Sponsor> CreateAsync(Sponsor sponsor)
        {
            // Validar nombre único
            var existing = await _sponsorRepository.GetByNameAsync(sponsor.Name);
            if (existing != null)
                throw new InvalidOperationException(
                    $"Ya existe un patrocinador con el nombre '{sponsor.Name}'");

            // Validar formato de email
            if (!IsValidEmail(sponsor.ContactEmail))
                throw new InvalidOperationException(
                    $"El email '{sponsor.ContactEmail}' no tiene un formato válido");

            _logger.LogInformation("Creating sponsor: {SponsorName}", sponsor.Name);
            return await _sponsorRepository.CreateAsync(sponsor);
        }

        public async Task UpdateAsync(int id, Sponsor sponsor)
        {
            var existing = await _sponsorRepository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException(
                    $"No se encontró el patrocinador con ID {id}");

            // Validar nombre único si cambió
            if (!existing.Name.Equals(sponsor.Name, StringComparison.OrdinalIgnoreCase))
            {
                var conflict = await _sponsorRepository.GetByNameAsync(sponsor.Name);
                if (conflict != null)
                    throw new InvalidOperationException(
                        $"Ya existe un patrocinador con el nombre '{sponsor.Name}'");
            }

            // Validar formato de email
            if (!IsValidEmail(sponsor.ContactEmail))
                throw new InvalidOperationException(
                    $"El email '{sponsor.ContactEmail}' no tiene un formato válido");

            existing.Name = sponsor.Name;
            existing.ContactEmail = sponsor.ContactEmail;
            existing.Phone = sponsor.Phone;
            existing.WebsiteUrl = sponsor.WebsiteUrl;
            existing.Category = sponsor.Category;

            _logger.LogInformation("Updating sponsor with ID: {SponsorId}", id);
            await _sponsorRepository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int id)
        {
            var exists = await _sponsorRepository.ExistsAsync(id);
            if (!exists)
                throw new KeyNotFoundException(
                    $"No se encontró el patrocinador con ID {id}");

            _logger.LogInformation("Deleting sponsor with ID: {SponsorId}", id);
            await _sponsorRepository.DeleteAsync(id);
        }

        public async Task LinkTournamentAsync(int sponsorId, int tournamentId, decimal contractAmount)
        {
            // Validar que el sponsor existe
            var sponsorExists = await _sponsorRepository.ExistsAsync(sponsorId);
            if (!sponsorExists)
                throw new KeyNotFoundException(
                    $"No se encontró el patrocinador con ID {sponsorId}");

            // Validar que el torneo existe
            var tournamentExists = await _tournamentRepository.ExistsAsync(tournamentId);
            if (!tournamentExists)
                throw new KeyNotFoundException(
                    $"No se encontró el torneo con ID {tournamentId}");

            // Validar que no esté ya vinculado
            var existing = await _tournamentSponsorRepository
                .GetByTournamentAndSponsorAsync(tournamentId, sponsorId);
            if (existing != null)
                throw new InvalidOperationException(
                    "Este patrocinador ya está vinculado a este torneo");

            // Validar ContractAmount > 0
            if (contractAmount <= 0)
                throw new InvalidOperationException(
                    "El monto del contrato debe ser mayor a 0");

            var tournamentSponsor = new TournamentSponsor
            {
                TournamentId = tournamentId,
                SponsorId = sponsorId,
                ContractAmount = contractAmount,
                JoinedAt = DateTime.UtcNow
            };

            _logger.LogInformation(
                "Linking sponsor {SponsorId} to tournament {TournamentId}",
                sponsorId, tournamentId);
            await _tournamentSponsorRepository.CreateAsync(tournamentSponsor);
        }

        public async Task UnlinkTournamentAsync(int sponsorId, int tournamentId)
        {
            var link = await _tournamentSponsorRepository
                .GetByTournamentAndSponsorAsync(tournamentId, sponsorId);
            if (link == null)
                throw new KeyNotFoundException(
                    "No existe vinculación entre este patrocinador y el torneo indicado");

            _logger.LogInformation(
                "Unlinking sponsor {SponsorId} from tournament {TournamentId}",
                sponsorId, tournamentId);
            await _tournamentSponsorRepository.DeleteAsync(link.Id);
        }

        public async Task<IEnumerable<TournamentSponsor>> GetTournamentsBySponsorAsync(int sponsorId)
        {
            var exists = await _sponsorRepository.ExistsAsync(sponsorId);
            if (!exists)
                throw new KeyNotFoundException(
                    $"No se encontró el patrocinador con ID {sponsorId}");

            return await _tournamentSponsorRepository.GetBySponsorIdAsync(sponsorId);
        }

        // ── HELPER PRIVADO: validar email ──
        private static bool IsValidEmail(string email) //se factoriza este metodo ya que lo usamos en varios lugares, asi evitamos repetir codigo y centralizamos la logica de validacion de email en un solo lugar, ademas de mejorar la legibilidad del codigo. estariamos aplicando el principio DRY (Don't Repeat Yourself).
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
