---
description: End-to-end integration checklist and testing guide
argument-hint: <optional: specific area to verify>
---

Run through the end-to-end integration checklist for BT Input.

## Prerequisites Check

```bash
# Phone side
cd phone && flutter analyze && flutter test

# PC side  
cd pc && dotnet build && dotnet test
```

## E2E Verification Checklist

### 1. BLE Connection

- [ ] PC app starts and begins BLE advertising/scanning
- [ ] Phone app discovers PC device in scan list
- [ ] Tap to connect succeeds within 10 seconds
- [ ] Both sides show "Connected" status
- [ ] MTU negotiation succeeds (check logs for negotiated MTU)

### 2. Basic Text Input

- [ ] Type a single Chinese character on phone → appears at PC cursor
- [ ] Type an English word → appears correctly
- [ ] Type numbers and symbols → appear correctly
- [ ] Latency feels < 100ms (subjective)

### 3. Delete Operations

- [ ] Backspace on phone → deletes character on PC
- [ ] Long-press backspace (rapid delete) → PC keeps up

### 4. Large Text

- [ ] Voice input 15+ chars → appears on PC (may use clipboard)
- [ ] Paste 50+ chars on phone → appears on PC via clipboard

### 5. Auto-Clear

- [ ] Input >500 chars + wait 2s → phone input box clears
- [ ] PC text is NOT affected by the clear
- [ ] Continued typing after clear works correctly

### 6. Hotkey & Floating Bar

- [ ] Ctrl+Shift+B toggles activation
- [ ] Floating bar appears when activated
- [ ] Floating bar shows interim text preview
- [ ] Floating bar does NOT steal focus from other apps
- [ ] Floating bar follows cursor position

### 7. Disconnect & Reconnect

- [ ] Turn off phone Bluetooth → PC shows "Disconnected"
- [ ] Turn on phone Bluetooth → auto-reconnects within 10s
- [ ] After reconnect, text input works immediately
- [ ] PC sends SYNC_REQUEST after reconnect; phone responds with FULLSYNC

### 8. Long Session (manual)

- [ ] 10 minutes continuous use — no crashes, no memory leaks
- [ ] Heartbeat visible in logs every 5s

## Focus Area

$ARGUMENTS
