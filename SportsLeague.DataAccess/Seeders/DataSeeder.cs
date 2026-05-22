using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;

namespace SportsLeague.DataAccess.Seeders
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(LeagueDbContext context)
        {
            // Solo ejecutar si no hay equipos (BD vacía)
            if (await context.Teams.AnyAsync()) return;

          
            // ═══ 1. EQUIPOS (Liga BetPlay 2026) ═══
            var teams = new List<Team>
            {
                new() { Name="Atlético Nacional", City="Medellín", Stadium="Atanasio Girardot" },
                new() { Name="Independiente Medellín", City="Medellín", Stadium="Atanasio Girardot" },
                new() { Name="América de Cali", City="Cali", Stadium="Pascual Guerrero" },
                new() { Name="Deportivo Cali", City="Cali", Stadium="Deportivo Cali" },
                new() { Name="Junior FC", City="Barranquilla", Stadium="Metropolitano" },
                new() { Name="Millonarios FC", City="Bogotá", Stadium="El Campín" },
                new() { Name="Independiente Santa Fe", City="Bogotá", Stadium="El Campín" },
                new() { Name="Deportes Tolima", City="Ibagué", Stadium="Manuel Murillo Toro" },
                new() { Name="Atlético Bucaramanga", City="Bucaramanga", Stadium="Alfonso López" },
                new() { Name="Once Caldas", City="Manizales", Stadium="Palogrande" },
                new() { Name="Deportivo Pasto", City="Pasto", Stadium="Departamental Libertad" },
                new() { Name="Deportivo Pereira", City="Pereira", Stadium="Hernán Ramírez Villegas" },
                new() { Name="Águilas Doradas", City="Rionegro", Stadium="Alberto Grisales" },
                new() { Name="Boyacá Chicó FC", City="Tunja", Stadium="La Independencia" },
                new() { Name="Jaguares de Córdoba", City="Montería", Stadium="Jaraguay" },
                new() { Name="Alianza Valledupar FC", City="Valledupar", Stadium="Armando Maestre" },
                new() { Name="Fortaleza FC", City="Bogotá", Stadium="Metropolitano de Techo" },
                new() { Name="Llaneros FC", City="Villavicencio", Stadium="Bello Horizonte" },
                new() { Name="Cúcuta Deportivo", City="Cúcuta", Stadium="General Santander" },
                new() { Name="Internacional de Bogotá", City="Bogotá", Stadium="Metropolitano de Techo" },
            };
            context.Teams.AddRange(teams);
            await context.SaveChangesAsync();


            // ═══ 2. JUGADORES (4 por equipo; Nacional y Medellín con 12 para pruebas de alineación) ═══
            var playersData = new (string First, string Last, PlayerPosition Pos, int Number)[][]
            {
            // 1. Atlético Nacional
                new[] {
                    ("David", "Ospina", PlayerPosition.Goalkeeper, 1),
                    ("William", "Tesillo", PlayerPosition.CentralDefender, 3),
                    ("Danovis", "Wanaguma", PlayerPosition.CentralDefender, 4),
                    ("Felipe", "Méndez", PlayerPosition.LeftBack, 5),
                    ("Emmanuel", "Ossa", PlayerPosition.RightBack, 2),
                    ("Jefferson", "Duque", PlayerPosition.DefensiveMidfielder, 6),
                    ("Edwin", "Cardona", PlayerPosition.AttackingMidfielder, 10),
                    ("Diber", "Cambindo", PlayerPosition.LeftWinger, 11),
                    ("Jhon", "Duque", PlayerPosition.RightWinger, 7),
                    ("Juan Pablo", "Solano", PlayerPosition.CentralMidfielder, 8),
                    ("Alfredo", "Morelos", PlayerPosition.Striker, 9),
                    ("Emanuel", "Mosquera", PlayerPosition.Striker, 19),
                },
            // 2. Independiente Medellín 
                new[] {
                    ("Salvador", "Ichazo", PlayerPosition.Goalkeeper, 1),
                    ("Andrés", "Cadavid", PlayerPosition.CentralDefender, 4),
                    ("Adrián", "Arregui", PlayerPosition.CentralMidfielder, 5),
                    ("Luciano", "Pons", PlayerPosition.Striker, 9),
                    ("Dorlan", "Pabón", PlayerPosition.LeftWinger, 11),
                    ("Baldomero", "Perlaza", PlayerPosition.RightBack, 2),
                    ("Jarlan", "Barrera", PlayerPosition.RightWinger, 7),
                    ("Sebastián", "Gómez", PlayerPosition.DefensiveMidfielder, 6),
                    ("Felipe", "Aguilar", PlayerPosition.CentralDefender, 14),
                    ("Jonatan", "Álvarez", PlayerPosition.Striker, 17),
                    ("Yairo", "Yepes", PlayerPosition.AttackingMidfielder, 10),
                    ("Jhon", "Lucumí", PlayerPosition.LeftBack, 3),
                },
            // 3. América de Cali
                new[] {
                    ("Joel", "Graterol", PlayerPosition.Goalkeeper, 1),
                    ("Jorge", "Segura", PlayerPosition.CentralDefender, 3),
                    ("Rodrigo", "Ureña", PlayerPosition.CentralMidfielder, 8),
                    ("Adrián", "Ramos", PlayerPosition.Striker, 9),
                },
            // 4. Deportivo Cali
                new[] {
                    ("Pedro", "Gallese", PlayerPosition.Goalkeeper, 1),
                    ("Fernando", "Álvarez", PlayerPosition.CentralDefender, 4),
                    ("Kevin", "Velasco", PlayerPosition.AttackingMidfielder, 10),
                    ("Juan", "Dinenno", PlayerPosition.Striker, 9),
                },
            // 5. Junior FC
                new[] {
                    ("Mauro", "Silveira", PlayerPosition.Goalkeeper, 1),
                    ("Edwin", "Herrera", PlayerPosition.RightBack, 4),
                    ("Fabián", "Ángel", PlayerPosition.CentralMidfielder, 8),
                    ("Carlos", "Bacca", PlayerPosition.Striker, 7),
                },
            // 6. Millonarios FC
                new[] {
                    ("Guillermo", "De Amores", PlayerPosition.Goalkeeper, 1),
                    ("Omar", "Bertel", PlayerPosition.LeftBack, 4),
                    ("Daniel", "Cataño", PlayerPosition.AttackingMidfielder, 10),
                    ("Leonardo", "Castro", PlayerPosition.Striker, 9),
                },
            // 7. Independiente Santa Fe
                new[] {
                    ("Leandro", "Castellanos", PlayerPosition.Goalkeeper, 1),
                    ("Elvis", "Mosquera", PlayerPosition.CentralDefender, 3),
                    ("Daniel", "Giraldo", PlayerPosition.DefensiveMidfielder, 5),
                    ("Hugo", "Rodallega", PlayerPosition.Striker, 9),
                },
            // 8. Deportes Tolima
                new[] {
                    ("William", "Cuesta", PlayerPosition.Goalkeeper, 1),
                    ("Jersson", "González", PlayerPosition.CentralDefender, 3),
                    ("Junior", "Hernández", PlayerPosition.AttackingMidfielder, 10),
                    ("Tatay", "Torres", PlayerPosition.Striker, 9),
                },
            // 9. Atlético Bucaramanga
                new[] {
                    ("Juan Camilo", "Chaverra", PlayerPosition.Goalkeeper, 1),
                    ("José", "Ortiz", PlayerPosition.Defender, 4),
                    ("Sherman", "Cárdenas", PlayerPosition.Midfielder, 10),
                    ("Sebastián", "Pons", PlayerPosition.Forward, 9),
                    },
            // 10. Once Caldas
                new[] {
                    ("Gerardo", "Ortiz", PlayerPosition.Goalkeeper, 1),
                    ("Edisson", "Palomino", PlayerPosition.Defender, 3),
                    ("Sebastián", "Gómez", PlayerPosition.Midfielder, 5),
                    ("Dayro", "Moreno", PlayerPosition.Forward, 9),
                    },
            // 11. Deportivo Pasto
                new[] {
                    ("Diego", "Martínez", PlayerPosition.Goalkeeper, 1),
                    ("Camilo", "Ayala", PlayerPosition.CentralDefender, 4),
                    ("Ray", "Vanegas", PlayerPosition.AttackingMidfielder, 10),
                    ("Jown", "Cardona", PlayerPosition.Striker, 9),
                },
            // 12. Deportivo Pereira
                new[] {
                    ("Harlen", "Castillo", PlayerPosition.Goalkeeper, 1),
                    ("David", "González", PlayerPosition.CentralDefender, 3),
                    ("Brayan", "León", PlayerPosition.CentralMidfielder, 8),
                    ("Jonier", "Mosquera", PlayerPosition.Striker, 9),
                },
            // 13. Águilas Doradas
                new[] {
                    ("José Fernando", "Cuadrado", PlayerPosition.Goalkeeper, 1),
                    ("Éder", "Chaux", PlayerPosition.LeftBack, 4),
                    ("Juan Pablo", "Ramírez", PlayerPosition.AttackingMidfielder, 10),
                    ("Cristian", "Subero", PlayerPosition.Striker, 9),
                },
            // 14. Boyacá Chicó FC
                new[] {
                    ("Ernesto", "Hernández", PlayerPosition.Goalkeeper, 1),
                    ("Carlos", "Henao", PlayerPosition.CentralDefender, 3),
                    ("Brayan", "Moreno", PlayerPosition.CentralMidfielder, 8),
                    ("Juan David", "Valencia", PlayerPosition.Striker, 9),
                },
            // 15. Jaguares de Córdoba
                new[] {
                    ("Diego", "Novoa", PlayerPosition.Goalkeeper, 1),
                    ("Geovan", "Montes", PlayerPosition.RightBack, 4),
                    ("Larry", "Vásquez", PlayerPosition.DefensiveMidfielder, 5),
                    ("Pablo", "Bueno", PlayerPosition.Striker, 9),
                },
            // 16. Alianza Valledupar FC
                new[] {
                    ("Luis", "Delgado", PlayerPosition.Goalkeeper, 1),
                    ("Marvin", "Vallecilla", PlayerPosition.CentralDefender, 3),
                    ("Juan", "Sánchez", PlayerPosition.CentralMidfielder, 8),
                    ("Jeison", "Medina", PlayerPosition.Striker, 9),
                },
            // 17. Fortaleza FC
                new[] {
                    ("Carlos", "Mosquera", PlayerPosition.Goalkeeper, 1),
                    ("Nicolás", "Giraldo", PlayerPosition.LeftBack, 4),
                    ("Jhonier", "Viveros", PlayerPosition.AttackingMidfielder, 10),
                    ("Óscar", "Vanegas", PlayerPosition.Striker, 9),
                },
            // 18. Llaneros FC
                new[] {
                    ("José Huber", "Escobar", PlayerPosition.Goalkeeper, 1),
                    ("Cristian", "Arrieta", PlayerPosition.CentralDefender, 3),
                    ("Jhon", "Pajoy", PlayerPosition.CentralMidfielder, 8),
                    ("Brayan", "Gil", PlayerPosition.Striker, 9),
                },
            // 19. Cúcuta Deportivo
                new[] {
                    ("Norberto", "Araujo", PlayerPosition.Goalkeeper, 1),
                    ("Jefry", "Díaz", PlayerPosition.RightBack, 4),
                    ("Juan Camilo", "Portilla", PlayerPosition.AttackingMidfielder, 10),
                    ("Edwar", "López", PlayerPosition.Striker, 9),
                },
                // 20. Internacional de Bogotá
                new[] {
                    ("Neto", "Volpi", PlayerPosition.Goalkeeper, 1),
                    ("Nicolás", "Hernández", PlayerPosition.CentralDefender, 3),
                    ("Carlos Darwin", "Quintero", PlayerPosition.AttackingMidfielder, 10),
                    ("Facundo", "Boné", PlayerPosition.Striker, 9),
                },
            };
            // playersData[i] corresponde a teams[i]
            var players = new List<Player>();
            for (int i = 0; i < teams.Count; i++)
            {
                foreach (var pd in playersData[i])
                {
                    players.Add(new Player
                    {
                        FirstName = pd.First,
                        LastName = pd.Last,
                        Number = pd.Number,
                        Position = pd.Pos,
                        BirthDate = new DateTime(1995, 1, 1).AddMonths(players.Count),
                        TeamId = teams[i].Id
                    });
                }
            }
            context.Players.AddRange(players);
            await context.SaveChangesAsync();

            // ═══ 3. ÁRBITROS ═══
            var referees = new List<Referee>
            {
                new() { FirstName="Wilmar", LastName="Roldán", Nationality="Colombia" },
                new() { FirstName="Andrés", LastName="Rojas", Nationality="Colombia" },
                new() { FirstName="Carlos", LastName="Betancur", Nationality="Colombia" },
                new() { FirstName="Jhon", LastName="Hinestroza", Nationality="Colombia" },
            };
            context.Referees.AddRange(referees);
            await context.SaveChangesAsync();

            // ═══ 4. TORNEO ═══
            var tournament = new Tournament
            {
                Name = "Liga BetPlay 2026-I",
                Season = "2026-I",
                StartDate = new DateTime(2026, 1, 16),
                EndDate = new DateTime(2026, 6, 5),
                Status = TournamentStatus.InProgress
            };
            context.Tournaments.Add(tournament);
            await context.SaveChangesAsync();

            // ═══ 5. INSCRIBIR LOS 20 EQUIPOS ═══
            foreach (var team in teams)
            {
                context.TournamentTeams.Add(new TournamentTeam
                {
                    TournamentId = tournament.Id,
                    TeamId = team.Id
                });
            }
            await context.SaveChangesAsync();

            // ═══ 6. PARTIDOS ═══
            // Partido 1: Nacional vs Medellín — Scheduled (para registrar alineaciones)
            var match1 = new Match
            {
                TournamentId = tournament.Id,
                HomeTeamId = teams[0].Id,  // Atlético Nacional
                AwayTeamId = teams[1].Id,  // Independiente Medellín
                RefereeId = referees[0].Id,
                MatchDate = new DateTime(2026, 2, 1, 15, 0, 0),
                Venue = "Atanasio Girardot",
                Matchday = 1,
                Status = MatchStatus.Scheduled
            };

            // Partido 2: América vs Cali — Scheduled (para pruebas adicionales de alineacion)
            var match2 = new Match
            {
                TournamentId = tournament.Id,
                HomeTeamId = teams[2].Id,  // América de Cali
                AwayTeamId = teams[3].Id,  // Deportivo Cali
                RefereeId = referees[1].Id,
                MatchDate = new DateTime(2026, 2, 1, 17, 0, 0),
                Venue = "Pascual Guerrero",
                Matchday = 1,
                Status = MatchStatus.Scheduled
            };

            // Partido 3: Junior vs Millonarios — Finished
            // Sirve para demostrar escenario negativo 6:
            // no se pueden registrar alineaciones en partidos Finished
            var match3 = new Match
            {
                TournamentId = tournament.Id,
                HomeTeamId = teams[4].Id,  // Junior FC
                AwayTeamId = teams[5].Id,  // Millonarios FC
                RefereeId = referees[2].Id,
                MatchDate = new DateTime(2026, 1, 20, 15, 0, 0),
                Venue = "Metropolitano",
                Matchday = 1,
                Status = MatchStatus.Finished
            };

            context.Matches.AddRange(match1, match2, match3);
            await context.SaveChangesAsync();

            // ═══ 7. RESULTADO DEL PARTIDO FINISHED ═══
            // Obligatorio porque el partido está Finished
            context.MatchResults.Add(new MatchResult
            {
                MatchId = match3.Id,
                HomeGoals = 2,
                AwayGoals = 1,
                Observations = "Partido de jornada 1"
            });
            await context.SaveChangesAsync();

            // ═══ 8. ALINEACIONES DEL PARTIDO 1 (Nacional vs Medellín) ═══
            // Orden por Player.Id (orden de inserción): Nacional 1–12, Medellín 13–24 en BD vacía
            // 10 titulares Nacional (Id 1–10); Id 11 y 12 libres para Swagger (11.º → 201, 12.º → 409)

            var nacionalPlayers = players
                .Where(p => p.TeamId == teams[0].Id)
                .OrderBy(p => p.Id)
                .ToList();
            var medellinPlayers = players
                .Where(p => p.TeamId == teams[1].Id)
                .OrderBy(p => p.Id)
                .ToList();

            var lineups = new List<MatchLineup>();

            for (var i = 0; i < 10; i++)
            {
                lineups.Add(new MatchLineup
                {
                    MatchId = match1.Id,
                    PlayerId = nacionalPlayers[i].Id,
                    IsStarter = true,
                    Position = nacionalPlayers[i].Position
                });
            }

            // nacionalPlayers[10] (Id 11) y [11] (Id 12) sin alinear

            lineups.Add(new MatchLineup
            {
                MatchId = match1.Id,
                PlayerId = medellinPlayers[0].Id,
                IsStarter = true,
                Position = medellinPlayers[0].Position
            });
            lineups.Add(new MatchLineup
            {
                MatchId = match1.Id,
                PlayerId = medellinPlayers[1].Id,
                IsStarter = true,
                Position = medellinPlayers[1].Position
            });
            lineups.Add(new MatchLineup
            {
                MatchId = match1.Id,
                PlayerId = medellinPlayers[2].Id,
                IsStarter = true,
                Position = medellinPlayers[2].Position
            });
            lineups.Add(new MatchLineup
            {
                MatchId = match1.Id,
                PlayerId = medellinPlayers[3].Id,
                IsStarter = false,
                Position = medellinPlayers[3].Position
            });

            context.MatchLineups.AddRange(lineups);
            await context.SaveChangesAsync();
        }
    }
}