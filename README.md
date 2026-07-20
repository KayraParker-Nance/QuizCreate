# QuizCreate
An app that converts formatted plain text from an AI model into an interactive HTML quiz.

## ToDo
- [ ] Improve UI
- [ ] Add dynamic window resizing
- [ ] Add copy template button
- [ ] Add save to button
- [ ] Add built in AI generation (?)
- [ ] Add mobile support

## AI Template (for generating app input)
Please generate multiple choice quiz questions from the content I provide.
Format your output EXACTLY as shown below — plain text only, no markdown,
no extra commentary, just the questions.

FORMAT:
SECTION: [Section or topic name]

Q: [Question text]
A: [Option 1]
B: [Option 2]
C: [Option 3]
D: [Option 4]
ANS: [Correct letter, e.g. B]
EXP: [One sentence explanation of why the answer is correct]

RULES:
- Leave a blank line between each question
- Use SECTION: to group related questions (optional if only one topic)
- Generate at least 15-20 questions per section / major topic
- Make distractors (wrong answers) plausible but clearly incorrect
- EXP should be concise and educational
- Output ONLY the formatted questions — no preamble, no summary at the end

Here is the content to generate questions from:
