---
description: Work on the Flutter phone app (phone/ directory)
argument-hint: <describe the task>
---

You are working on the **phone/** Flutter project for BT Input.

## Context Loading

1. Read `CLAUDE.md` for project overview and conventions
2. If the task involves BLE communication or protocol messages, also read `docs/PROTOCOL.md`
3. If the task involves UI layout or interaction, also read `docs/PRD.md` section 3 (UI/UX)
4. If the task involves DiffEngine, ThrottledDiffSender, or core algorithms, also read `docs/LOW_LEVEL_DESIGN.md`

## Key Source Files

```
phone/lib/
├── main.dart                    # App entry point
├── app.dart                     # MaterialApp config
├── pages/
│   ├── connection_page.dart     # BLE device scanning & pairing
│   ├── input_page.dart          # Main input UI (core page)
│   └── settings_page.dart       # Settings & device management
├── services/
│   ├── ble_service.dart         # BLE GATT server & communication
│   └── connection_manager.dart  # Connection state & auto-reconnect
├── core/
│   ├── diff_engine.dart         # Text diff algorithm (prefix+suffix)
│   ├── throttle_sender.dart     # 50ms throttled diff sender
│   └── protocol.dart            # Message encoding (JSON)
├── models/
│   ├── text_delta.dart          # TextDelta data class
│   ├── device_info.dart         # BLE device info
│   └── connection_state.dart    # Connection state enum
└── utils/
    ├── constants.dart           # UUIDs, thresholds, timeouts
    └── logger.dart              # Logging utility
```

## Workflow

1. Implement the requested changes
2. Run `cd phone && flutter analyze` — fix all issues
3. Run `cd phone && flutter test` — fix all failures
4. Run `cd phone && dart format lib/ test/` — ensure formatting
5. Summarize what was done

## Rules

- Use `flutter_blue_plus` for all BLE operations
- All public APIs must have dartdoc comments (`///`)
- Write unit tests for new logic (not for UI pages)
- Use `final` and immutable patterns wherever possible
- State management: keep it simple — `StatefulWidget` + `setState` for MVP; no Riverpod/Bloc yet

## Task

$ARGUMENTS
