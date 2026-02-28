---
description: Test and validate the BLE protocol implementation on both sides
argument-hint: <specific area to test, or "all" for full validation>
---

You are validating the BLE communication protocol between phone and PC for BT Input.

## Context

Read `docs/PROTOCOL.md` for the full protocol specification.
Read `CLAUDE.md` for GATT service UUIDs and message format.

## What to Validate

### 1. Message Encoding (Phone side)

Verify `phone/lib/core/protocol.dart` correctly encodes all message types:

- TEXT_DELTA with all 4 ops (A/I/D/R) + clipboard hint
- TEXT_FULLSYNC
- HEARTBEAT
- SEGMENT_COMPLETE
Run: `cd phone && flutter test test/core/protocol_test.dart`

### 2. Message Decoding (PC side)

Verify `pc/src/Core/ProtocolDecoder.cs` correctly decodes all message types.
Run: `cd pc && dotnet test --filter "Protocol"`

### 3. DiffEngine Scenarios

Verify all 7 input scenarios produce correct deltas:

```
A: Pinyin char-by-char    →  APPEND per character
B: Voice whole sentence   →  APPEND with clipboard_hint=true
C: Candidate replacement  →  REPLACE
D: Auto-complete          →  APPEND
E1: Tail delete           →  DELETE at end
E2: Middle delete         →  DELETE at position
F: Select-all paste       →  FULL_SYNC (change > 60%)
G: Middle insert          →  INSERT at position
```

Run: `cd phone && flutter test test/core/diff_engine_test.dart`

### 4. Throttle Behavior

Verify ThrottledDiffSender:

- First change fires immediately (0ms delay)
- Subsequent changes within 50ms window are batched
- Last state always gets sent when window closes
Run: `cd phone && flutter test test/core/throttle_sender_test.dart`

### 5. Sequence Number Handling

Verify PC side:

- Normal sequential processing (seq 1, 2, 3...)
- Gap detection (got seq=5 when expecting seq=3 → SYNC_REQUEST)
- Wrap-around (65535 → 0)

### 6. Cross-validation

If both sides are implemented, write an integration test that:

- Encodes a TEXT_DELTA on phone side (Dart)
- Decodes the same JSON bytes on PC side (C#)
- Verifies identical semantics

## Area to Focus

$ARGUMENTS
