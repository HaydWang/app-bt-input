---
description: Save session context before clearing — enables seamless continuation in a new session
---

Before we end this session, create a handoff document.

Write a file `docs/SESSION_HANDOFF.md` with the following sections:

## 1. What Was Accomplished

List every file created or modified in this session, with a one-line description of the change.

## 2. Current State

- What is working and tested?
- What compiles but is untested?
- What is stubbed/placeholder?

## 3. Known Issues

List any bugs, compiler warnings, failing tests, or TODO items you are aware of.

## 4. Next Steps

What are the immediate next tasks to continue development? Order by priority.
Reference the Phase Plan from CLAUDE.md.

## 5. Key Decisions Made

List any design decisions or trade-offs made during this session that a future session should know about.

## 6. Context for Continuation

A 3-5 sentence summary that a fresh Claude Code session can read to quickly understand where we left off.

---

After writing the file, run:

```
git add -A
git status
```

Show me the status so I can review before committing.
