# Genera Scripts/seed-torneo-base.sql a partir de DataBase/seed-utn-golmundial-2026.json
# (RF28). No se ejecuta automaticamente en ningun lado, es solo la herramienta
# de generacion; el .sql resultante es el artefacto versionado que se entrega.

$ErrorActionPreference = "Stop"
$jsonPath = "C:\Users\MSI GAMING\Descargas\3°\Proyect_Integrator\DataBase\seed-utn-golmundial-2026.json"
$outPath = "C:\Users\MSI GAMING\Descargas\3°\Proyect_Integrator\VSCom\UTNGolMundial2026\EstadisticasAPI\Scripts\seed-torneo-base.sql"

$data = Get-Content $jsonPath -Raw | ConvertFrom-Json

function Esc([string]$s) {
    if ($null -eq $s) { return "" }
    return $s.Replace("'", "''")
}

$sb = New-Object System.Text.StringBuilder
[void]$sb.AppendLine("-- ============================================================")
[void]$sb.AppendLine("-- Seed base del torneo — UTN GolMundial 2026 (RF28)")
[void]$sb.AppendLine("-- Generado automaticamente desde DataBase/seed-utn-golmundial-2026.json")
[void]$sb.AppendLine("-- (script Scripts/generar-seed.ps1). NO editar a mano: regenerar desde el JSON.")
[void]$sb.AppendLine("-- Carga: 16 sedes, 48 selecciones, 72 partidos de fase de grupos.")
[void]$sb.AppendLine("-- Uso: psql -U postgres -d utn_estadisticas -f seed-torneo-base.sql")
[void]$sb.AppendLine("-- ============================================================")
[void]$sb.AppendLine("")
[void]$sb.AppendLine("BEGIN;")
[void]$sb.AppendLine("")

# SEDES
[void]$sb.AppendLine('-- Sedes')
foreach ($s in $data.sedes) {
    [void]$sb.AppendLine("INSERT INTO ""Sedes"" (""Id"", ""Nombre"", ""Ciudad"", ""Pais"", ""Capacidad"") VALUES ($($s.id), '$(Esc $s.nombre)', '$(Esc $s.ciudad)', '$(Esc $s.pais)', $($s.capacidadAprox)) ON CONFLICT (""Id"") DO NOTHING;")
}
[void]$sb.AppendLine("")

# SELECCIONES
[void]$sb.AppendLine('-- Selecciones')
foreach ($sel in $data.selecciones) {
    [void]$sb.AppendLine("INSERT INTO ""Selecciones"" (""Id"", ""Nombre"", ""Codigo"", ""Grupo"", ""Escudo"", ""PartidosJugados"", ""PartidosGanados"", ""PartidosEmpatados"", ""PartidosPerdidos"", ""GolesFavor"", ""GolesContra"", ""Puntos"") VALUES ($($sel.id), '$(Esc $sel.nombre)', '$(Esc $sel.codigoFifa)', '$(Esc $sel.grupo)', '', 0, 0, 0, 0, 0, 0, 0) ON CONFLICT (""Id"") DO NOTHING;")
}
[void]$sb.AppendLine("")

# PARTIDOS (fase de grupos)
[void]$sb.AppendLine('-- Partidos (fase de grupos)')
foreach ($p in $data.partidos) {
    # Se usa el valor "fase" literal del JSON del docente (ej. "GRUPOS"), igual
    # que ya está cargado en la BD real — nada en el código filtra por este
    # valor exacto salvo el endpoint de eliminatorias, que usa otros nombres.
    $fase = $p.fase
    # "estado" SIEMPRE en minúscula ("programado"), sin importar el valor del
    # JSON ("PROGRAMADO"): así lo esperan RegistrarResultado/ActualizarEstado.
    $estado = "programado"
    # Formato ISO 8601 explícito con 'Z' — evita que Postgres interprete la
    # fecha en el TimeZone de la sesión o con el orden de fecha equivocado.
    $fechaIso = $p.fechaHoraUtc.ToString("yyyy-MM-ddTHH:mm:ss") + "Z"
    [void]$sb.AppendLine("INSERT INTO ""Partidos"" (""Id"", ""SeleccionLocalId"", ""SeleccionVisitanteId"", ""SedeId"", ""FechaHora"", ""Fase"", ""Grupo"", ""Estado"", ""GolesLocal"", ""GolesVisitante"") VALUES ($($p.id), $($p.seleccionLocalId), $($p.seleccionVisitanteId), $($p.sedeId), '$fechaIso', '$fase', '$(Esc $p.grupo)', '$estado', NULL, NULL) ON CONFLICT (""Id"") DO NOTHING;")
}
[void]$sb.AppendLine("")

# Resincronizar las secuencias de IDENTITY tras insertar IDs explícitos
[void]$sb.AppendLine('-- Resincronizar secuencias IDENTITY para que los próximos INSERT (vía API) no choquen con estos IDs')
[void]$sb.AppendLine('SELECT setval(pg_get_serial_sequence(''"Sedes"'', ''Id''), COALESCE((SELECT MAX("Id") FROM "Sedes"), 1));')
[void]$sb.AppendLine('SELECT setval(pg_get_serial_sequence(''"Selecciones"'', ''Id''), COALESCE((SELECT MAX("Id") FROM "Selecciones"), 1));')
[void]$sb.AppendLine('SELECT setval(pg_get_serial_sequence(''"Partidos"'', ''Id''), COALESCE((SELECT MAX("Id") FROM "Partidos"), 1));')
[void]$sb.AppendLine("")
[void]$sb.AppendLine("COMMIT;")

Set-Content -Path $outPath -Value $sb.ToString() -Encoding UTF8

$sedesCount = $data.sedes.Count
$seleccionesCount = $data.selecciones.Count
$partidosCount = $data.partidos.Count
Write-Output "Generado: $outPath"
Write-Output "Sedes: $sedesCount | Selecciones: $seleccionesCount | Partidos: $partidosCount"
