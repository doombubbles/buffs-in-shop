# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [1.1.4] - 2026-06-03

- Updated for BTD6 v55
- Internally switched to using some newer Mod Helper extensions

## [1.1.3] - 2026-05-26

- Updated internals to use the new `ModMutator` class from Mod Helper 3.6.0
- Fixed some minor bugs from v54

## [1.1.2] - 2025-12-03

- Fixed for v52

## [1.1.1] - 2025-11-13

- Fixed a console error showing up that didn't actually affect anything

## [1.1.0] - 2025-11-06

- Added many Hero buffs, Paragons Buffs, and stackable tower buffs to the shop
- Added a new "God Boost" buff that applies all other buffs to a tower at once
- Fixed Take Better Aim being applyable to a tower without Take Aim
- Hid some buffs from the shop in Chimps that don't actually do anything
- Added settings for allowing buffs to individually bypass the Require Buff Origin Usable setting

## [1.0.4] - 2025-10-15

- Updated for BTD6 v51
- Added a new setting Always Affect Sub Towers, default false
  - Makes the buffs that don't normally affect sub-towers like Alchemists' and Overclock now affect sub-towers.

## [1.0.3] - 2025-09-09

- Fixed some interactions with how Buffs apply to subtowers
- Fixed Absolute Zero placement effect
- Made Cost settings no longer require a restart to change

## [1.0.2] - 2025-08-31

- Fixed some interactions between certain buffs and Ability Choice and Mega Knowledge effects

## [1.0.1] - 2025-08-31

- All Buffs in Shop are now immune to being absorbed by Lych
- Fixed an error that could happen with `GetPrimaryWeaponThrowMarkerHeight`
- Fixed a potential error when the internal CHIMPs mutator would modify the game model

## [1.0.0] - 2025-08-27

Initial Release

[unreleased]: https://github.com/doombubbles/BuffsInShop/compare/1.1.4...HEAD
[1.1.4]: https://github.com/doombubbles/BuffsInShop/compare/1.1.3...1.1.4
[1.1.3]: https://github.com/doombubbles/BuffsInShop/compare/1.1.2...1.1.3
[1.1.2]: https://github.com/doombubbles/BuffsInShop/compare/1.1.1...1.1.2
[1.1.1]: https://github.com/doombubbles/BuffsInShop/compare/1.1.0...1.1.1
[1.1.0]: https://github.com/doombubbles/BuffsInShop/compare/1.0.4...1.1.0
[1.0.4]: https://github.com/doombubbles/BuffsInShop/compare/1.0.3...1.0.4
[1.0.3]: https://github.com/doombubbles/BuffsInShop/compare/1.0.2...1.0.3
[1.0.2]: https://github.com/doombubbles/BuffsInShop/compare/1.0.1...1.0.2
[1.0.1]: https://github.com/doombubbles/BuffsInShop/compare/1.0.0...1.0.1
[1.0.0]: https://github.com/doombubbles/BuffsInShop/compare/3864a01c5a63e8de1d5a6aa8d995c143c4a95f54...1.0.0
