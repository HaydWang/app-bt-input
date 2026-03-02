import 'package:fake_async/fake_async.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:phone/core/throttle_sender.dart';
import 'package:phone/models/text_delta.dart';

void main() {
  group('ThrottledDiffSender', () {
    test('first change sends immediately', () {
      fakeAsync((async) {
        final sent = <TextDelta>[];
        final sender = ThrottledDiffSender(
          onSend: sent.add,
          throttleWindow: const Duration(milliseconds: 50),
        );

        sender.onTextChanged('你');

        expect(sent.length, 1);
        expect(sent.first.op, DeltaOp.append);
        async.elapse(const Duration(milliseconds: 60));
      });
    });

    test('changes in window are merged to latest', () {
      fakeAsync((async) {
        final sent = <TextDelta>[];
        final sender = ThrottledDiffSender(
          onSend: sent.add,
          throttleWindow: const Duration(milliseconds: 50),
        );

        sender.onTextChanged('你');
        sender.onTextChanged('你好');
        sender.onTextChanged('你好世');

        expect(sent.length, 1);
        expect(sent[0].text, '你');

        async.elapse(const Duration(milliseconds: 50));

        expect(sent.length, 2);
        expect(sent[1].op, DeltaOp.append);
        expect(sent[1].text, '好世');
      });
    });

    test('same buffered text does not send duplicate', () {
      fakeAsync((async) {
        final sent = <TextDelta>[];
        final sender = ThrottledDiffSender(
          onSend: sent.add,
          throttleWindow: const Duration(milliseconds: 50),
        );

        sender.onTextChanged('abc');
        sender.onTextChanged('abc');

        async.elapse(const Duration(milliseconds: 50));

        expect(sent.length, 1);
      });
    });

    test('reset clears throttle and diff state', () {
      fakeAsync((async) {
        final sent = <TextDelta>[];
        final sender = ThrottledDiffSender(
          onSend: sent.add,
          throttleWindow: const Duration(milliseconds: 50),
        );

        sender.onTextChanged('abc');
        sender.reset();
        sender.onTextChanged('x');

        expect(sent.length, 2);
        expect(sent[1].op, DeltaOp.append);
        expect(sent[1].position, 0);
        expect(sent[1].text, 'x');

        async.elapse(const Duration(milliseconds: 60));
      });
    });
  });
}
