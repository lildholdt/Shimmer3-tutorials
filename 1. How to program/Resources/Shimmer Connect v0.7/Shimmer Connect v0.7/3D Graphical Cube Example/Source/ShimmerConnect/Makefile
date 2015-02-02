.PHONY: linux
linux:
	xbuild /p:Configuration=Release /p:DefineConstants=_PLATFORM_LINUX ShimmerConnect.sln

.PHONY: windows
windows:
	xbuild /p:Configuration=Release ShimmerConnect.sln

.PHONY: clean
clean:
	rm -rf ShimmerConnect.userprefs ShimmerConnect.suo ShimmerConnect/*.pidb ShimmerConnect/*.resources ShimmerConnect/bin ShimmerConnect/obj ShimmerConnect/Properties/*.resources
