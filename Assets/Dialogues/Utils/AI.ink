EXTERNAL AIAnswer(varName, system, question, max_tokens)
EXTERNAL AIGenerateText(varName, prompt, max_tokens)
EXTERNAL AIGenerateImage(varName, prompt, w, h)
EXTERNAL AITalk(system, max_tokens)
EXTERNAL AIWaitFor(varName)

// these are global vars, change them before calling a function
VAR ai_temperature = 1.2
VAR ai_style = "ANIME"
VAR ai_allow_censored_images = 0
