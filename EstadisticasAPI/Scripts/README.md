# Seed del torneo (RF28)

`seed-torneo-base.sql` carga el calendario base provisto por el docente:
16 sedes, 48 selecciones (12 grupos) y los 72 partidos de fase de grupos.

`seed-eliminatorias.sql` agrega los 32 partidos de fases eliminatorias
(Dieciseisavos a Final) con los emparejamientos y resultados reales del
Mundial 2026 — exportados de la base de datos real ya cargada manualmente
en una sesión anterior (no vienen en el JSON del docente, ver más abajo).

Juntos suman los 104 partidos totales que exige RF04.

## Uso

Con la base `utn_estadisticas` ya creada y las migraciones de EF Core aplicadas
(`dotnet ef database update`, tablas `Sedes`/`Selecciones`/`Partidos` existentes
y vacías):

```
psql -U postgres -d utn_estadisticas -f seed-torneo-base.sql
psql -U postgres -d utn_estadisticas -f seed-eliminatorias.sql
```

Ambos son idempotentes (`ON CONFLICT ("Id") DO NOTHING`): correrlos dos veces no duplica filas.
`seed-eliminatorias.sql` depende de que `seed-torneo-base.sql` haya corrido antes
(referencia por FK a las 48 selecciones y 16 sedes).

## Regenerar el script

Si el JSON de origen (`DataBase/seed-utn-golmundial-2026.json`) cambia,
regenerar con:

```
pwsh Scripts/generar-seed.ps1
```

No editar `seed-torneo-base.sql` a mano — es un artefacto generado.

## Por qué las eliminatorias no vienen en el JSON del docente

Los 32 partidos de fases eliminatorias (Dieciseisavos a Final) **no** están
en `DataBase/seed-utn-golmundial-2026.json` — su metadata lo dice explícitamente:
`"partidosIncluidosEnSeed": "Fase de grupos (72 partidos)"`. Es estructural, no
un descuido: los emparejamientos de eliminatorias dependen de quién califica
(1°/2° de cada grupo + 8 mejores terceros), algo que solo se sabe después de
jugarse la fase de grupos real, por lo que no puede fijarse en un seed estático.
`seed-eliminatorias.sql` cubre ese resto exportando lo que ya se cargó a mano
con los resultados reales del torneo.

## Regenerar seed-eliminatorias.sql

A diferencia de `seed-torneo-base.sql` (que se regenera desde el JSON), este
archivo se generó una vez exportando directamente la tabla `Partidos` de
`utn_estadisticas` (`WHERE "Fase" IN ('Dieciseisavos','Octavos','Cuartos',
'Semifinal','TercerPuesto','Final')`). Si el bracket de eliminatorias cambia
más adelante (nuevos resultados), hay que volver a exportarlo con la misma
consulta y reemplazar este archivo.
