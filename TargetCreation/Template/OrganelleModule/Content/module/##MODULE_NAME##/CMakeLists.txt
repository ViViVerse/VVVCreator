#/**************************************************************************************************************************************************************/
# *
# *  Build target
# *
#/**************************************************************************************************************************************************************/



add_library(
	##MODULE_NAME##_mod
	SHARED
	source/##MODULE_NAME##.cpp
	header/pch.h
	)



if( WIN32 )
	target_sources(
		##MODULE_NAME##_mod
		PRIVATE
		source/pch.cpp
		resource/##MODULE_NAME##.rc
		)
endif()



set_target_properties(##MODULE_NAME##_mod PROPERTIES PREFIX "")
set_target_properties(##MODULE_NAME##_mod PROPERTIES OUTPUT_NAME "##MODULE_NAME##")
set_target_properties(##MODULE_NAME##_mod PROPERTIES SUFFIX ".vvvorg")



#/**************************************************************************************************************************************************************\
# *
# *  Linked libraries
# *
#\**************************************************************************************************************************************************************/



if( WIN32 )
	target_link_libraries(
		##MODULE_NAME##_mod
		kernel32
		user32
		gdi32
		winspool
		comdlg32
		advapi32
		shell32
		ole32
		oleaut32
		uuid
		odbc32
		odbccp32
		vvv_mathematics
		##MODULE_NAME##
		##MODULE_NAME##_symbiosis
		vvv_quanta
		vvv_core_symbiosis
		vvv_essence
		vvv_protein
		vvv_base_symbiosis
		)
elseif( UNIX )
	target_link_libraries(
		##MODULE_NAME##_mod
		-Wl,--start-group
		pthread
		dl
		ssl
		crypto
		uuid
		##MODULE_NAME##
		##MODULE_NAME##_symbiosis
		vvv_protein
		vvv_essence
		vvv_core_symbiosis
		vvv_quanta
		-Wl,--end-group
		)
endif()



#/**************************************************************************************************************************************************************/
# *
# *  Output directory
# *
#/**************************************************************************************************************************************************************/



set_target_properties(
	##MODULE_NAME##_mod
	PROPERTIES
	ARCHIVE_OUTPUT_DIRECTORY ${VVV_BinaryDir}/${VVV_BuildPlatformDir}/${VVV_BuildConfigDir}
	LIBRARY_OUTPUT_DIRECTORY ${VVV_BinaryDir}/${VVV_BuildPlatformDir}/${VVV_BuildConfigDir}
	RUNTIME_OUTPUT_DIRECTORY ${VVV_BinaryDir}/${VVV_BuildPlatformDir}/${VVV_BuildConfigDir}
	)



#/**************************************************************************************************************************************************************/
# *
# *  Include directories
# *
#/**************************************************************************************************************************************************************/



target_include_directories(##MODULE_NAME##_mod PRIVATE ${CMAKE_CURRENT_SOURCE_DIR}/../../collection/include)
target_include_directories(##MODULE_NAME##_mod PRIVATE ${CMAKE_CURRENT_SOURCE_DIR}/../../common/include)



#/**************************************************************************************************************************************************************/
# *
# *  Compiler Flags
# *
#/**************************************************************************************************************************************************************/



if( "${CMAKE_CXX_COMPILER_ID}" STREQUAL "GNU" )
	target_compile_options(##MODULE_NAME##_mod PRIVATE "-O0")

	target_compile_options(##MODULE_NAME##_mod PRIVATE "-g2")
	target_compile_options(##MODULE_NAME##_mod PRIVATE "-gdwarf-2")

	target_compile_options(##MODULE_NAME##_mod PRIVATE "-w")
	target_compile_options(##MODULE_NAME##_mod PRIVATE "-Wswitch")
	target_compile_options(##MODULE_NAME##_mod PRIVATE "-Wno-deprecated-declarations")
	target_compile_options(##MODULE_NAME##_mod PRIVATE "-Wempty-body")
	target_compile_options(##MODULE_NAME##_mod PRIVATE "-Wconversion")
	target_compile_options(##MODULE_NAME##_mod PRIVATE "-Wreturn-type")
	target_compile_options(##MODULE_NAME##_mod PRIVATE "-Wparentheses")
	target_compile_options(##MODULE_NAME##_mod PRIVATE "-Wno-format")
	target_compile_options(##MODULE_NAME##_mod PRIVATE "-Wuninitialized")
	target_compile_options(##MODULE_NAME##_mod PRIVATE "-Wunreachable-code")
	target_compile_options(##MODULE_NAME##_mod PRIVATE "-Wunused-function")
	target_compile_options(##MODULE_NAME##_mod PRIVATE "-Wunused-value")
	target_compile_options(##MODULE_NAME##_mod PRIVATE "-Wunused-variable")

	target_compile_options(##MODULE_NAME##_mod PRIVATE "-fno-strict-aliasing")
	target_compile_options(##MODULE_NAME##_mod PRIVATE "-fno-omit-frame-pointer")
	target_compile_options(##MODULE_NAME##_mod PRIVATE "-fthreadsafe-statics")
	target_compile_options(##MODULE_NAME##_mod PRIVATE "-fexceptions")
	target_compile_options(##MODULE_NAME##_mod PRIVATE "-fno-rtti")
	target_compile_options(##MODULE_NAME##_mod PRIVATE "-fpermissive")
	target_compile_options(##MODULE_NAME##_mod PRIVATE "-fPIC")
	target_compile_options(##MODULE_NAME##_mod PRIVATE "-fno-keep-static-consts")

	target_compile_options(##MODULE_NAME##_mod PRIVATE "-std=c++1z")
elseif( "${CMAKE_CXX_COMPILER_ID}" STREQUAL "MSVC" )
	target_compile_options(##MODULE_NAME##_mod PRIVATE "/GS")
	target_compile_options(##MODULE_NAME##_mod PRIVATE "/GL")
	target_compile_options(##MODULE_NAME##_mod PRIVATE "/analyze-")
	target_compile_options(##MODULE_NAME##_mod PRIVATE "/W3")
	target_compile_options(##MODULE_NAME##_mod PRIVATE "/wd5030")
	target_compile_options(##MODULE_NAME##_mod PRIVATE "/Zc:wchar_t")
	target_compile_options(##MODULE_NAME##_mod PRIVATE "/Zi")
	target_compile_options(##MODULE_NAME##_mod PRIVATE "/Gm-")
	target_compile_options(##MODULE_NAME##_mod PRIVATE "/Zc:inline")
	target_compile_options(##MODULE_NAME##_mod PRIVATE "/fp:precise")
	target_compile_options(##MODULE_NAME##_mod PRIVATE "/errorReport:prompt")
	target_compile_options(##MODULE_NAME##_mod PRIVATE "/GF")
	target_compile_options(##MODULE_NAME##_mod PRIVATE "/WX-")
	target_compile_options(##MODULE_NAME##_mod PRIVATE "/Zc:forScope")
	target_compile_options(##MODULE_NAME##_mod PRIVATE "/arch:AVX")
	target_compile_options(##MODULE_NAME##_mod PRIVATE "/Gd")
	target_compile_options(##MODULE_NAME##_mod PRIVATE "/Oy-")
	target_compile_options(##MODULE_NAME##_mod PRIVATE "/MDd")
	target_compile_options(##MODULE_NAME##_mod PRIVATE "/FC")
	target_compile_options(##MODULE_NAME##_mod PRIVATE "/diagnostics:classic")
	target_compile_options(##MODULE_NAME##_mod PRIVATE "/EHsc")
endif()



#/**************************************************************************************************************************************************************/
# *
# *  Linker Flags Flags
# *
#/**************************************************************************************************************************************************************/



if( "${CMAKE_CXX_COMPILER_ID}" STREQUAL "GNU" )
    target_link_options(##MODULE_NAME##_mod PRIVATE "-Wl,--no-undefined")
endif()