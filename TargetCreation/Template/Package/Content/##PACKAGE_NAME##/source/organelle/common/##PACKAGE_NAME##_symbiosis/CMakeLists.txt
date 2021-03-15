#/**************************************************************************************************************************************************************/
# *
# *  Build target
# *
#/**************************************************************************************************************************************************************/



add_library(
	##PACKAGE_NAME##_symbiosis
	STATIC
	source/##UND_PACKAGE_NAME##_package.cpp
	source/pch.cpp
	header/pch.h
	../include/##PACKAGE_NAME##_exception.h
	../include/##PACKAGE_NAME##_package.h
	../include/##PACKAGE_NAME##_symbiosis.h
	)



#/**************************************************************************************************************************************************************/
# *
# *  Output directory
# *
#/**************************************************************************************************************************************************************/



set_target_properties(
	##PACKAGE_NAME##_symbiosis
	PROPERTIES
	ARCHIVE_OUTPUT_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}/../library/${VVV_BuildPlatformDir}/${VVV_BuildConfigDir}
	LIBRARY_OUTPUT_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}/../library/${VVV_BuildPlatformDir}/${VVV_BuildConfigDir}
	RUNTIME_OUTPUT_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}/../library/${VVV_BuildPlatformDir}/${VVV_BuildConfigDir}
	)



#/**************************************************************************************************************************************************************/
# *
# *  Include directories
# *
#/**************************************************************************************************************************************************************/



target_include_directories(##PACKAGE_NAME##_symbiosis PRIVATE ${CMAKE_CURRENT_SOURCE_DIR}/../../common/include)
#target_include_directories(##PACKAGE_NAME## PRIVATE ${VVV_PackageDir}/vvv_???other package/source/organelle/common/include)



#/**************************************************************************************************************************************************************/
# *
# *  Compiler Flags
# *
#/**************************************************************************************************************************************************************/



if( "${CMAKE_CXX_COMPILER_ID}" STREQUAL "GNU" )
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "-D_VVV_##uc(UND_PACKAGE_NAME##_SYMBIOSIS_LIB_BUILD")

	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "-O0")

	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "-g2")
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "-gdwarf-2")

	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "-w")
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "-Wswitch")
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "-Wno-deprecated-declarations")
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "-Wempty-body")
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "-Wconversion")
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "-Wreturn-type")
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "-Wparentheses")
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "-Wno-format")
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "-Wuninitialized")
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "-Wunreachable-code")
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "-Wunused-function")
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "-Wunused-value")
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "-Wunused-variable")

	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "-fno-strict-aliasing")
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "-fno-omit-frame-pointer")
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "-fthreadsafe-statics")
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "-fexceptions")
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "-fno-rtti")
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "-fpermissive")
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "-fPIC")
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "-fno-keep-static-consts")

	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "-std=c++1z")
elseif( "${CMAKE_CXX_COMPILER_ID}" STREQUAL "MSVC" )
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "/D_VVV_##uc(UND_PACKAGE_NAME##_SYMBIOSIS_LIB_BUILD")
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "/GS")
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "/GL")
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "/analyze-")
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "/W3")
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "/wd5030")
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "/Zc:wchar_t")
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "/Zi")
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "/Gm-")
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "/Zc:inline")
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "/fp:precise")
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "/errorReport:prompt")
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "/GF")
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "/WX-")
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "/Zc:forScope")
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "/arch:AVX")
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "/Gd")
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "/Oy-")
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "/MDd")
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "/FC")
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "/diagnostics:classic")
	target_compile_options(##PACKAGE_NAME##_symbiosis PRIVATE "/EHsc")
endif()
