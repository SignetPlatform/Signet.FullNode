echo Mining... Press [CTRL+C] to stop
:loop
	dotnet signet.cli.dll signet POST Mining/generate BlockCount=1
goto loop