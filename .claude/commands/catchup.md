---
description: Resume development from a previous session — reads handoff notes and current code state
---

We are continuing development on BT Input from a previous session.

## Step 1: Load Context

1. Read `CLAUDE.md` for project overview
2. Read `docs/SESSION_HANDOFF.md` if it exists — this contains notes from the last session
3. Run `git log --oneline -10` to see recent commits
4. Run `git diff --stat` to see any uncommitted changes

## Step 2: Assess Current State

For the phone app:

```bash
cd phone && flutter analyze 2>&1 | tail -20
cd phone && flutter test 2>&1 | tail -20
```

For the PC app:

```bash
cd pc && dotnet build 2>&1 | tail -20
cd pc && dotnet test 2>&1 | tail -20
```

## Step 3: Report

Give me a concise status report:

- ✅ What's working
- ⚠️ What needs attention (warnings, failing tests)
- 📋 Recommended next task from the Phase Plan

Then ask me what I'd like to work on.
