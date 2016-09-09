# pkmn-world

Offline "single player" geolocation game built in Unity.

- Join [our Discord server](https://discord.gg/ztxpvkM).
- Let us [know what you can do](https://goo.gl/forms/uIIVtHmDl7roj2YB3).

When you start working on something, please create an issue (or comment on one if there is one already).

## To Do
### Map
- [x] Geolocation
- [x] Render map around player from OSM data
- [ ] Generate centers and shops from Map POIs

### Encounters
- [ ] Track player distance moved/speed
- [x] Query 3rd party data sources (weather etc)
- [x] Generate encounters based on available data
  - [ ] Altitude
  - [x] Time
  - [ ] Date/Season
  - [x] Weather
  - [x] Landuse/Landcover
  - [ ] Waterway
- [ ] Keep encounters on map for set time/until interacted
- [ ] Destroy old/far away encounters
- [ ] Tracking of certain pokemon from nearby UI

### In-game data
- [ ] Save player stats
- [ ] Set name
- [ ] Set home location
- [ ] Save captured mons
- [ ] Save party
- [ ] Save pokedex
- [ ] Save inventory

### UI
- [ ] Map nearby screen
  - [ ] Display landuse, weather, temperature etc
  - [ ] Display likely monsters to spawn

- [ ] Party screen
  - [ ] Display party mons
  - [ ] Party reordering
  - [ ] Party management when at home/pokecenter

- [ ] Inventory
  - [ ] Display items
  - [ ] Item Management
  - [ ] Item details

### Graphics
- [ ] Modular attack move animation system
- [ ] Mons sprites/models
- [ ] Map materials

### Data
- [ ] Pokedex
  - [x] Encounter locations/conditions
  - [ ] Rarity
- [ ] Move list/effects
- [ ] Item list/effects

### Battles
- [ ] Battle scene
- [ ] Battle engine
  - [ ] Timing system
  - [ ] Logic

### Networking
- [ ] Investigate peer to peer local networking
- [ ] Socket server

### Development
- [ ] Build process documentation
- [ ] Proper issue/work tracking

## Useful links

- [Vector Map in Unity](http://barankahyaoglu.com/dev/pokemongo-clone-using-mapzen-api-unity3d/)
- [OSM Vector Maps](https://mapzen.com/documentation/vector-tiles/)
- [Design Document](https://docs.google.com/document/d/14CyEM0dSjxGzEMtS2bPMSw9HAmsaR4V2AvKk3L-1uWE/edit?usp=sharing)
- [Who knows what](https://docs.google.com/spreadsheets/d/1iO5sxFpMURuBBN6vaJGAEQ8YlfGaF4rltjR47Q4KDkg/edit?usp=sharing)
- [This repo](https://github.com/pkmn-world/pkmn-world)
- [Spawn Tester in Python](https://github.com/pkmn-world/test-spawner)
- [Pokemon Data](http://pokeapi.co/) [Repo](https://github.com/PokeAPI/pokeapi)
