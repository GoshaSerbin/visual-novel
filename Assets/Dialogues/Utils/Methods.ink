EXTERNAL GetChoice(choices, num)
EXTERNAL AskPlayer(varName)
EXTERNAL Fight()

=== function max(a,b) ===
	{ a < b:
		~ return b
	- else:
		~ return a
	}
	
=== function min(a,b) ===
	{ a > b:
		~ return b
	- else:
		~ return a
	}