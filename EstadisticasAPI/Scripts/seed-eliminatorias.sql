-- ============================================================
-- Seed de fases eliminatorias — UTN GolMundial 2026
-- Exportado desde la base de datos real (utn_estadisticas) el 2026-07-23.
-- Complementa a seed-torneo-base.sql (que solo trae los 72 partidos de
-- fase de grupos, tal como lo entrega el docente): estos son los 32
-- partidos de Dieciseisavos a Final, con los emparejamientos y resultados
-- reales ya cargados manualmente en una sesión anterior.
--
-- Ejecutar DESPUÉS de seed-torneo-base.sql (referencia por FK a Selecciones
-- y Sedes ya sembradas ahí). Idempotente (ON CONFLICT DO NOTHING).
-- Uso: psql -U postgres -d utn_estadisticas -f seed-eliminatorias.sql
-- ============================================================

BEGIN;

INSERT INTO "Partidos" ("Id", "SeleccionLocalId", "SeleccionVisitanteId", "SedeId", "FechaHora", "Fase", "Grupo", "Estado", "GolesLocal", "GolesVisitante") VALUES (89, 3, 5, 7, '2026-06-28T19:00:00Z', 'Dieciseisavos', 'M73', 'finalizado', 0, 1) ON CONFLICT ("Id") DO NOTHING;
INSERT INTO "Partidos" ("Id", "SeleccionLocalId", "SeleccionVisitanteId", "SedeId", "FechaHora", "Fase", "Grupo", "Estado", "GolesLocal", "GolesVisitante") VALUES (90, 9, 22, 10, '2026-06-29T19:00:00Z', 'Dieciseisavos', 'M76', 'finalizado', 2, 1) ON CONFLICT ("Id") DO NOTHING;
INSERT INTO "Partidos" ("Id", "SeleccionLocalId", "SeleccionVisitanteId", "SedeId", "FechaHora", "Fase", "Grupo", "Estado", "GolesLocal", "GolesVisitante") VALUES (91, 17, 15, 11, '2026-06-29T16:00:00Z', 'Dieciseisavos', 'M74', 'finalizado', 1, 2) ON CONFLICT ("Id") DO NOTHING;
INSERT INTO "Partidos" ("Id", "SeleccionLocalId", "SeleccionVisitanteId", "SedeId", "FechaHora", "Fase", "Grupo", "Estado", "GolesLocal", "GolesVisitante") VALUES (92, 21, 10, 3, '2026-06-29T22:00:00Z', 'Dieciseisavos', 'M75', 'finalizado', 1, 2) ON CONFLICT ("Id") DO NOTHING;
INSERT INTO "Partidos" ("Id", "SeleccionLocalId", "SeleccionVisitanteId", "SedeId", "FechaHora", "Fase", "Grupo", "Estado", "GolesLocal", "GolesVisitante") VALUES (93, 19, 35, 8, '2026-06-30T19:00:00Z', 'Dieciseisavos', 'M78', 'finalizado', 1, 2) ON CONFLICT ("Id") DO NOTHING;
INSERT INTO "Partidos" ("Id", "SeleccionLocalId", "SeleccionVisitanteId", "SedeId", "FechaHora", "Fase", "Grupo", "Estado", "GolesLocal", "GolesVisitante") VALUES (94, 33, 24, 6, '2026-06-30T16:00:00Z', 'Dieciseisavos', 'M77', 'finalizado', 3, 0) ON CONFLICT ("Id") DO NOTHING;
INSERT INTO "Partidos" ("Id", "SeleccionLocalId", "SeleccionVisitanteId", "SedeId", "FechaHora", "Fase", "Grupo", "Estado", "GolesLocal", "GolesVisitante") VALUES (95, 1, 18, 1, '2026-06-30T22:00:00Z', 'Dieciseisavos', 'M79', 'finalizado', 2, 0) ON CONFLICT ("Id") DO NOTHING;
INSERT INTO "Partidos" ("Id", "SeleccionLocalId", "SeleccionVisitanteId", "SedeId", "FechaHora", "Fase", "Grupo", "Estado", "GolesLocal", "GolesVisitante") VALUES (96, 45, 44, 9, '2026-07-01T19:00:00Z', 'Dieciseisavos', 'M80', 'finalizado', 2, 1) ON CONFLICT ("Id") DO NOTHING;
INSERT INTO "Partidos" ("Id", "SeleccionLocalId", "SeleccionVisitanteId", "SedeId", "FechaHora", "Fase", "Grupo", "Estado", "GolesLocal", "GolesVisitante") VALUES (97, 25, 34, 16, '2026-07-01T22:00:00Z', 'Dieciseisavos', 'M82', 'finalizado', 3, 2) ON CONFLICT ("Id") DO NOTHING;
INSERT INTO "Partidos" ("Id", "SeleccionLocalId", "SeleccionVisitanteId", "SedeId", "FechaHora", "Fase", "Grupo", "Estado", "GolesLocal", "GolesVisitante") VALUES (98, 13, 6, 15, '2026-07-01T16:00:00Z', 'Dieciseisavos', 'M81', 'finalizado', 2, 0) ON CONFLICT ("Id") DO NOTHING;
INSERT INTO "Partidos" ("Id", "SeleccionLocalId", "SeleccionVisitanteId", "SedeId", "FechaHora", "Fase", "Grupo", "Estado", "GolesLocal", "GolesVisitante") VALUES (99, 29, 38, 7, '2026-07-02T19:00:00Z', 'Dieciseisavos', 'M84', 'finalizado', 3, 0) ON CONFLICT ("Id") DO NOTHING;
INSERT INTO "Partidos" ("Id", "SeleccionLocalId", "SeleccionVisitanteId", "SedeId", "FechaHora", "Fase", "Grupo", "Estado", "GolesLocal", "GolesVisitante") VALUES (100, 41, 46, 4, '2026-07-02T16:00:00Z', 'Dieciseisavos', 'M83', 'finalizado', 2, 1) ON CONFLICT ("Id") DO NOTHING;
INSERT INTO "Partidos" ("Id", "SeleccionLocalId", "SeleccionVisitanteId", "SedeId", "FechaHora", "Fase", "Grupo", "Estado", "GolesLocal", "GolesVisitante") VALUES (101, 8, 39, 5, '2026-07-02T22:00:00Z', 'Dieciseisavos', 'M85', 'finalizado', 2, 0) ON CONFLICT ("Id") DO NOTHING;
INSERT INTO "Partidos" ("Id", "SeleccionLocalId", "SeleccionVisitanteId", "SedeId", "FechaHora", "Fase", "Grupo", "Estado", "GolesLocal", "GolesVisitante") VALUES (102, 14, 27, 8, '2026-07-03T16:00:00Z', 'Dieciseisavos', 'M88', 'finalizado', 1, 2) ON CONFLICT ("Id") DO NOTHING;
INSERT INTO "Partidos" ("Id", "SeleccionLocalId", "SeleccionVisitanteId", "SedeId", "FechaHora", "Fase", "Grupo", "Estado", "GolesLocal", "GolesVisitante") VALUES (103, 37, 32, 13, '2026-07-03T19:00:00Z', 'Dieciseisavos', 'M86', 'finalizado', 3, 2) ON CONFLICT ("Id") DO NOTHING;
INSERT INTO "Partidos" ("Id", "SeleccionLocalId", "SeleccionVisitanteId", "SedeId", "FechaHora", "Fase", "Grupo", "Estado", "GolesLocal", "GolesVisitante") VALUES (104, 42, 48, 14, '2026-07-03T22:00:00Z', 'Dieciseisavos', 'M87', 'finalizado', 1, 0) ON CONFLICT ("Id") DO NOTHING;
INSERT INTO "Partidos" ("Id", "SeleccionLocalId", "SeleccionVisitanteId", "SedeId", "FechaHora", "Fase", "Grupo", "Estado", "GolesLocal", "GolesVisitante") VALUES (105, 5, 10, 10, '2026-07-04T17:00:00Z', 'Octavos', 'M90', 'finalizado', 0, 3) ON CONFLICT ("Id") DO NOTHING;
INSERT INTO "Partidos" ("Id", "SeleccionLocalId", "SeleccionVisitanteId", "SedeId", "FechaHora", "Fase", "Grupo", "Estado", "GolesLocal", "GolesVisitante") VALUES (106, 15, 33, 12, '2026-07-04T21:00:00Z', 'Octavos', 'M89', 'finalizado', 0, 1) ON CONFLICT ("Id") DO NOTHING;
INSERT INTO "Partidos" ("Id", "SeleccionLocalId", "SeleccionVisitanteId", "SedeId", "FechaHora", "Fase", "Grupo", "Estado", "GolesLocal", "GolesVisitante") VALUES (107, 9, 35, 6, '2026-07-05T20:00:00Z', 'Octavos', 'M91', 'finalizado', 1, 2) ON CONFLICT ("Id") DO NOTHING;
INSERT INTO "Partidos" ("Id", "SeleccionLocalId", "SeleccionVisitanteId", "SedeId", "FechaHora", "Fase", "Grupo", "Estado", "GolesLocal", "GolesVisitante") VALUES (108, 1, 45, 1, '2026-07-06T00:00:00Z', 'Octavos', 'M92', 'finalizado', 2, 3) ON CONFLICT ("Id") DO NOTHING;
INSERT INTO "Partidos" ("Id", "SeleccionLocalId", "SeleccionVisitanteId", "SedeId", "FechaHora", "Fase", "Grupo", "Estado", "GolesLocal", "GolesVisitante") VALUES (109, 41, 29, 8, '2026-07-06T19:00:00Z', 'Octavos', 'M93', 'finalizado', 0, 1) ON CONFLICT ("Id") DO NOTHING;
INSERT INTO "Partidos" ("Id", "SeleccionLocalId", "SeleccionVisitanteId", "SedeId", "FechaHora", "Fase", "Grupo", "Estado", "GolesLocal", "GolesVisitante") VALUES (110, 13, 25, 16, '2026-07-07T00:00:00Z', 'Octavos', 'M94', 'finalizado', 1, 4) ON CONFLICT ("Id") DO NOTHING;
INSERT INTO "Partidos" ("Id", "SeleccionLocalId", "SeleccionVisitanteId", "SedeId", "FechaHora", "Fase", "Grupo", "Estado", "GolesLocal", "GolesVisitante") VALUES (111, 37, 27, 9, '2026-07-07T16:00:00Z', 'Octavos', 'M95', 'finalizado', 3, 2) ON CONFLICT ("Id") DO NOTHING;
INSERT INTO "Partidos" ("Id", "SeleccionLocalId", "SeleccionVisitanteId", "SedeId", "FechaHora", "Fase", "Grupo", "Estado", "GolesLocal", "GolesVisitante") VALUES (112, 8, 42, 5, '2026-07-07T20:00:00Z', 'Octavos', 'M96', 'finalizado', 1, 0) ON CONFLICT ("Id") DO NOTHING;
INSERT INTO "Partidos" ("Id", "SeleccionLocalId", "SeleccionVisitanteId", "SedeId", "FechaHora", "Fase", "Grupo", "Estado", "GolesLocal", "GolesVisitante") VALUES (113, 33, 10, 11, '2026-07-09T20:00:00Z', 'Cuartos', 'M97', 'finalizado', 2, 0) ON CONFLICT ("Id") DO NOTHING;
INSERT INTO "Partidos" ("Id", "SeleccionLocalId", "SeleccionVisitanteId", "SedeId", "FechaHora", "Fase", "Grupo", "Estado", "GolesLocal", "GolesVisitante") VALUES (114, 29, 25, 7, '2026-07-10T19:00:00Z', 'Cuartos', 'M98', 'finalizado', 2, 1) ON CONFLICT ("Id") DO NOTHING;
INSERT INTO "Partidos" ("Id", "SeleccionLocalId", "SeleccionVisitanteId", "SedeId", "FechaHora", "Fase", "Grupo", "Estado", "GolesLocal", "GolesVisitante") VALUES (115, 35, 45, 13, '2026-07-11T21:00:00Z', 'Cuartos', 'M99', 'finalizado', 1, 2) ON CONFLICT ("Id") DO NOTHING;
INSERT INTO "Partidos" ("Id", "SeleccionLocalId", "SeleccionVisitanteId", "SedeId", "FechaHora", "Fase", "Grupo", "Estado", "GolesLocal", "GolesVisitante") VALUES (116, 37, 8, 14, '2026-07-12T01:00:00Z', 'Cuartos', 'M100', 'finalizado', 3, 1) ON CONFLICT ("Id") DO NOTHING;
INSERT INTO "Partidos" ("Id", "SeleccionLocalId", "SeleccionVisitanteId", "SedeId", "FechaHora", "Fase", "Grupo", "Estado", "GolesLocal", "GolesVisitante") VALUES (117, 29, 33, 8, '2026-07-14T19:00:00Z', 'Semifinal', 'SF1', 'finalizado', 2, 0) ON CONFLICT ("Id") DO NOTHING;
INSERT INTO "Partidos" ("Id", "SeleccionLocalId", "SeleccionVisitanteId", "SedeId", "FechaHora", "Fase", "Grupo", "Estado", "GolesLocal", "GolesVisitante") VALUES (118, 37, 45, 9, '2026-07-15T19:00:00Z', 'Semifinal', 'SF2', 'finalizado', 2, 1) ON CONFLICT ("Id") DO NOTHING;
INSERT INTO "Partidos" ("Id", "SeleccionLocalId", "SeleccionVisitanteId", "SedeId", "FechaHora", "Fase", "Grupo", "Estado", "GolesLocal", "GolesVisitante") VALUES (119, 45, 33, 13, '2026-07-18T21:00:00Z', 'TercerPuesto', '3ER', 'finalizado', 6, 4) ON CONFLICT ("Id") DO NOTHING;
INSERT INTO "Partidos" ("Id", "SeleccionLocalId", "SeleccionVisitanteId", "SedeId", "FechaHora", "Fase", "Grupo", "Estado", "GolesLocal", "GolesVisitante") VALUES (120, 29, 37, 6, '2026-07-19T19:00:00Z', 'Final', 'FINAL', 'programado', 1, 0) ON CONFLICT ("Id") DO NOTHING;

-- Resincronizar la secuencia IDENTITY de Partidos (estos IDs, 89-120, son
-- más altos que los 72 de fase de grupos).
SELECT setval(pg_get_serial_sequence('"Partidos"', 'Id'), COALESCE((SELECT MAX("Id") FROM "Partidos"), 1));

COMMIT;
