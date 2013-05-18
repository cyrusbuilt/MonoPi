
EXTRA_DIST =  Makefile

# Warning: This is an automatically generated file, do not edit!

srcdir=.
top_srcdir=..

include $(top_srcdir)/config.make

ifeq ($(CONFIG),DEBUG)
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG;"
ASSEMBLY = bin/Debug/CyrusBuilt.MonoPi.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES = 
BUILD_DIR = bin/Debug

CYRUSBUILT_MONOPI_DLL_MDB_SOURCE=bin/Debug/CyrusBuilt.MonoPi.dll.mdb
CYRUSBUILT_MONOPI_DLL_MDB=$(BUILD_DIR)/CyrusBuilt.MonoPi.dll.mdb

endif

ifeq ($(CONFIG),RELEASE)
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+
ASSEMBLY = bin/Release/CyrusBuilt.MonoPi.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES = 
BUILD_DIR = bin/Release

CYRUSBUILT_MONOPI_DLL_MDB=

endif

AL=al
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(CYRUSBUILT_MONOPI_DLL_MDB)  

LINUX_PKGCONFIG = \
	$(CYRUSBUILT_MONOPI_PC)  


RESGEN=resgen2

CYRUSBUILT_MONOPI_PC = $(BUILD_DIR)/cyrusbuilt.monopi.pc

FILES =  \
	AssemblyInfo.cs \
	BoardRevision.cs \
	Gertboard/GertboardAdcChannels.cs \
	Gertboard/GertboardDevice.cs \
	I2C/I2CBus.cs \
	I2C/II2CBus.cs \
	IO/GpioBase.cs \
	IO/GpioFile.cs \
	IO/GpioMem.cs \
	IO/GpioPins.cs \
	IO/IGpio.cs \
	IO/PinDirection.cs \
	IO/Serial/BaudRates.cs \
	IO/Serial/Rs232SerialPort.cs \
	LCD/DisplayEntryModes.cs \
	LCD/DisplayOnOffControl.cs \
	LCD/FunctionSetFlags.cs \
	LCD/GpioFileLcdTransferProvider.cs \
	LCD/GpioMemLcdTransferProvider.cs \
	LCD/ILcdTransferProvider.cs \
	LCD/LcdCommands.cs \
	LCD/LcdModule.cs \
	LED/TM1638.cs \
	LED/TM1638LedColor.cs \
	LED/TM16XXBase.cs \
	RTC/DS1302.cs \
	Sensors/DS1620.cs \
	SPI/AdcChannels.cs \
	SPI/MCP3008.cs \
	SPI/SimpleSPI.cs \
	UnsafeNativeMethods.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	IO \
	IO/Serial \
	Sensors \
	SPI \
	LED \
	LCD \
	Gertboard \
	I2C \
	RTC \
	cyrusbuilt.monopi.pc.in 

REFERENCES =  \
	Mono.Posix \
	System

DLL_REFERENCES = 

CLEANFILES = $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

#Targets
all: $(ASSEMBLY) $(PROGRAMFILES) $(LINUX_PKGCONFIG)  $(top_srcdir)/config.make

include $(top_srcdir)/Makefile.include
#include $(srcdir)/custom-hooks.make



$(eval $(call emit-deploy-wrapper,CYRUSBUILT_MONOPI_PC,cyrusbuilt.monopi.pc))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'


$(ASSEMBLY_MDB): $(ASSEMBLY)
$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	make pre-all-local-hook prefix=$(prefix)
	mkdir -p $(shell dirname $(ASSEMBLY))
	make $(CONFIG)_BeforeBuild
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
	make $(CONFIG)_AfterBuild
	make post-all-local-hook prefix=$(prefix)

install-local: $(ASSEMBLY) $(ASSEMBLY_MDB)
	make pre-install-local-hook prefix=$(prefix)
	make install-satellite-assemblies prefix=$(prefix)
	mkdir -p '$(DESTDIR)$(libdir)/$(PACKAGE)'
	$(call cp,$(ASSEMBLY),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(ASSEMBLY_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(CYRUSBUILT_MONOPI_DLL_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	mkdir -p '$(DESTDIR)$(libdir)/pkgconfig'
	$(call cp,$(CYRUSBUILT_MONOPI_PC),$(DESTDIR)$(libdir)/pkgconfig)
	make post-install-local-hook prefix=$(prefix)

uninstall-local: $(ASSEMBLY) $(ASSEMBLY_MDB)
	make pre-uninstall-local-hook prefix=$(prefix)
	make uninstall-satellite-assemblies prefix=$(prefix)
	$(call rm,$(ASSEMBLY),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(ASSEMBLY_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(CYRUSBUILT_MONOPI_DLL_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(CYRUSBUILT_MONOPI_PC),$(DESTDIR)$(libdir)/pkgconfig)
	make post-uninstall-local-hook prefix=$(prefix)

install: install-local
uninstall: uninstall-local
clean: clean-local

include $(top_srcdir)/rules.make
