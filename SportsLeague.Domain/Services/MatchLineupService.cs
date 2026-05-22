using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;
using SportsLeague.Domain.Helpers;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace SportsLeague.Domain.Services
{
    public class MatchLineupService : IMatchLineupService
    {
        private readonly IMatchLineupRepository _lineupRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly MatchValidationHelper _validationHelper;
        private readonly ILogger<MatchLineupService> _logger;

        public MatchLineupService(
            IMatchLineupRepository lineupRepository,
            IMatchRepository matchRepository,
            MatchValidationHelper validationHelper,
            ILogger<MatchLineupService> logger)
        {
            _lineupRepository = lineupRepository; 
            _matchRepository = matchRepository;
            _validationHelper = validationHelper;
            _logger = logger;
        }

        public async Task<MatchLineup> AddPlayerToLineupAsync(int matchId, MatchLineup lineup)
        {
            // V1: El partido debe existir
            var match = await _matchRepository.GetByIdAsync(matchId);
            if (match == null)
                throw new KeyNotFoundException(
                    $"No se encontró el partido con ID {matchId}");

            // V6: El partido debe estar en estado Scheduled
            if (match.Status != MatchStatus.Scheduled)
                throw new InvalidOperationException(
                    "Solo se pueden registrar alineaciones en partidos Scheduled");

           
            // ValidatePlayerInMatchAsync valida existencia del jugador
            // y que pertenece a uno de los dos equipos del partido
            // V2 + V3

            var player = await _validationHelper.ValidatePlayerInMatchAsync(
                lineup.PlayerId, match); 

            // V4: El jugador no puede estar registrado dos veces en la misma alineación
            var alreadyRegistered = await _lineupRepository
                .ExistsByMatchAndPlayerAsync(matchId, lineup.PlayerId);
            if (alreadyRegistered)
                throw new InvalidOperationException(
                    "El jugador ya está registrado en la alineación de este partido");

            // V5: Máximo 11 titulares por equipo por partido
            // Solo aplica si el jugador va como titular (IsStarter = true)
            // Los suplentes no tienen límite
            if (lineup.IsStarter)
            {
                var starterCount = await _lineupRepository
                    .CountStartersByMatchAndTeamAsync(
                        matchId,
                        player.TeamId,
                        isStarter: true);

                if (starterCount >= 11)
                    throw new InvalidOperationException(
                        "El equipo ya tiene 11 titulares registrados en este partido");
            }

            lineup.MatchId = matchId;

            _logger.LogInformation(
                "Adding player {PlayerId} to lineup of match {MatchId} as {Role}",
                lineup.PlayerId,
                matchId,
                lineup.IsStarter ? "starter" : "substitute");

            return await _lineupRepository.CreateAsync(lineup);
        }

        public async Task<IEnumerable<MatchLineup>> GetLineupByMatchAsync(int matchId)
        {
            var match = await _matchRepository.GetByIdAsync(matchId);
            if (match == null)
                throw new KeyNotFoundException(
                    $"No se encontró el partido con ID {matchId}");

            return await _lineupRepository.GetByMatchAsync(matchId);
        }
        // Obtener alineación por partido y equipo
        public async Task<IEnumerable<MatchLineup>> GetLineupByMatchAndTeamAsync(
            int matchId, int teamId)
        {
            var match = await _matchRepository.GetByIdAsync(matchId);
            if (match == null)
                throw new KeyNotFoundException(
                    $"No se encontró el partido con ID {matchId}");

            if (match.HomeTeamId != teamId && match.AwayTeamId != teamId)
                throw new InvalidOperationException(
                    "El equipo especificado no participa en este partido");

            return await _lineupRepository.GetByMatchAndTeamAsync(matchId, teamId);
        }

        public async Task DeleteFromLineupAsync(int id)
        {
            var exists = await _lineupRepository.ExistsAsync(id);
            if (!exists)
                throw new KeyNotFoundException(
                    $"No se encontró el registro de alineación con ID {id}");

            _logger.LogInformation(
                "Removing lineup entry with ID: {LineupId}", id);

            await _lineupRepository.DeleteAsync(id);
        }
    }
}