enum DeltaOp {
  append,
  insert,
  delete,
  replace,
  fullSync,
  noChange,
}

class TextDelta {
  const TextDelta({
    required this.op,
    this.position = 0,
    this.deleteCount = 0,
    this.text = '',
    this.clipboardHint = false,
  });

  // TODO: Expand this model when protocol mapping is implemented.
  final DeltaOp op;
  final int position;
  final int deleteCount;
  final String text;
  final bool clipboardHint;
}
