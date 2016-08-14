
SampleCode.dll:
	mcs SampleCode.cs -target:library

SampleCodeDocs: SampleCode.dll
	mdoc update SampleCode.dll -o SampleCodeDocs

SampleCode.tree: update
	mdoc assemble -o SampleCode SampleCodeDocs
	mkdir -p Monodoc.Razor.Tests/bin/Debug/sources
	mkdir -p Monodoc.Razor.Tests/bin/Release/sources
	cp SampleCode.tree Monodoc.Razor.Tests/bin/Debug/sources
	cp SampleCode.tree Monodoc.Razor.Tests/bin/Release/sources
	cp SampleCode.zip Monodoc.Razor.Tests/bin/Debug/sources
	cp SampleCode.zip Monodoc.Razor.Tests/bin/Release/sources
	cp SampleCode.source Monodoc.Razor.Tests/bin/Debug/sources
	cp SampleCode.source Monodoc.Razor.Tests/bin/Release/sources
	cp monodoc.xml Monodoc.Razor.Tests/bin/Debug/
	cp monodoc.xml Monodoc.Razor.Tests/bin/Release/

build:SampleCode.dll
update: SampleCodeDocs
assemble: SampleCode.tree

clean: 
	rm SampleCode.dll SampleCode.tree SampleCode.zip
	rm -rf SampleCodeDocs
	rm -rf Monodoc.Razor.Tests/bin
	rm -rf Monodoc.Razor.Tests/obj