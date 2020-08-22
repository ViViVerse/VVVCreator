#/**************************************************************************************************************************************************************/
# *
# *  General
# *
#/**************************************************************************************************************************************************************/



project(##PACKAGE_NAME##)



##USE_QT?##
#/**************************************************************************************************************************************************************/
# *
# *  Qt Moc
# *
#/**************************************************************************************************************************************************************/



if( USE_QT )
    set(CMAKE_AUTOMOC ON)
    set(CMAKE_AUTOUIC ON)
    #Since the addition of ${VVV_PackageDir} to vvv_all.props.cmake include_directories(), the moc generation has a problem with pch.h
    #files situated in one of the subfolders of the package folder. see also vvv_visual_qt.
    if(${CMAKE_VERSION} VERSION_LESS "3.16.0")
        set(CMAKE_AUTOMOC_MOC_OPTIONS "-b${CMAKE_CURRENT_SOURCE_DIR}/header/pch.h")
    else()
        set(CMAKE_AUTOMOC_MOC_OPTIONS "-bpch.h")
    endif()
endif()



##}##
#/**************************************************************************************************************************************************************/
# *
# *  Build target
# *
#/**************************************************************************************************************************************************************/



add_library(
	##PACKAGE_NAME##
	STATIC
	source/pch.cpp
	header/pch.h
	../include/##PACKAGE_NAME##_library.h
	)



#/**************************************************************************************************************************************************************/
# *
# *  Output directory
# *
#/**************************************************************************************************************************************************************/



set_target_properties(
	##PACKAGE_NAME##
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



target_include_directories(##PACKAGE_NAME## PRIVATE ${CMAKE_CURRENT_SOURCE_DIR}/../../collection/include)
target_include_directories(##PACKAGE_NAME## PRIVATE ${CMAKE_CURRENT_SOURCE_DIR}/../../common/include)
#target_include_directories(##PACKAGE_NAME## PRIVATE ${VVV_PackageDir}/vvv_???other package/source/organelle/common/include)



#/**************************************************************************************************************************************************************/
# *
# *  Compiler Flags
# *
#/**************************************************************************************************************************************************************/



if( "${CMAKE_CXX_COMPILER_ID}" STREQUAL "GNU" )
	target_compile_options(##PACKAGE_NAME## PRIVATE "-D_VVV_ORGANELLE_LIB_BUILD")
##USE_QT?##
	if( USE_QT )
		target_compile_options(vvv_qt PRIVATE "-DVVV_USE_QT")
		target_compile_options(vvv_qt PRIVATE "-DQT_DEPRECATED_WARNINGS")
		target_compile_options(vvv_qt PRIVATE "-DQT_NO_DEBUG")
		target_compile_options(vvv_qt PRIVATE "-DQT_WIDGETS_LIB")
		target_compile_options(vvv_qt PRIVATE "-DQT_CORE_LIB")
	endif()
##}##
	target_compile_options(##PACKAGE_NAME## PRIVATE "-O0")
	target_compile_options(##PACKAGE_NAME## PRIVATE "-g2")
	target_compile_options(##PACKAGE_NAME## PRIVATE "-gdwarf-2")
	target_compile_options(##PACKAGE_NAME## PRIVATE "-w")
	target_compile_options(##PACKAGE_NAME## PRIVATE "-Wswitch")
	target_compile_options(##PACKAGE_NAME## PRIVATE "-Wno-deprecated-declarations")
	target_compile_options(##PACKAGE_NAME## PRIVATE "-Wempty-body")
	target_compile_options(##PACKAGE_NAME## PRIVATE "-Wconversion")
	target_compile_options(##PACKAGE_NAME## PRIVATE "-Wreturn-type")
	target_compile_options(##PACKAGE_NAME## PRIVATE "-Wparentheses")
	target_compile_options(##PACKAGE_NAME## PRIVATE "-Wno-format")
	target_compile_options(##PACKAGE_NAME## PRIVATE "-Wuninitialized")
	target_compile_options(##PACKAGE_NAME## PRIVATE "-Wunreachable-code")
	target_compile_options(##PACKAGE_NAME## PRIVATE "-Wunused-function")
	target_compile_options(##PACKAGE_NAME## PRIVATE "-Wunused-value")
	target_compile_options(##PACKAGE_NAME## PRIVATE "-Wunused-variable")
	target_compile_options(##PACKAGE_NAME## PRIVATE "-fno-strict-aliasing")
	target_compile_options(##PACKAGE_NAME## PRIVATE "-fno-omit-frame-pointer")
	target_compile_options(##PACKAGE_NAME## PRIVATE "-fthreadsafe-statics")
	target_compile_options(##PACKAGE_NAME## PRIVATE "-fexceptions")
	target_compile_options(##PACKAGE_NAME## PRIVATE "-fno-rtti")
	target_compile_options(##PACKAGE_NAME## PRIVATE "-fpermissive")
	target_compile_options(##PACKAGE_NAME## PRIVATE "-fPIC")
	target_compile_options(##PACKAGE_NAME## PRIVATE "-fno-keep-static-consts")

	target_compile_options(##PACKAGE_NAME## PRIVATE "-std=c++1z")
elseif( "${CMAKE_CXX_COMPILER_ID}" STREQUAL "MSVC" )
	target_compile_options(##PACKAGE_NAME## PRIVATE "/D_VVV_ORGANELLE_LIB_BUILD")
##USE_QT?##
	if( USE_QT )
		target_compile_options(vvv_qt PRIVATE "/DVVV_USE_QT")
		target_compile_options(vvv_qt PRIVATE "/DQT_DEPRECATED_WARNINGS")
		target_compile_options(vvv_qt PRIVATE "/DQT_NO_DEBUG")
		target_compile_options(vvv_qt PRIVATE "/DQT_WIDGETS_LIB")
		target_compile_options(vvv_qt PRIVATE "/DQT_CORE_LIB")
	endif()
##}##
	target_compile_options(##PACKAGE_NAME## PRIVATE "/GS")
	target_compile_options(##PACKAGE_NAME## PRIVATE "/GL")
	target_compile_options(##PACKAGE_NAME## PRIVATE "/analyze-")
	target_compile_options(##PACKAGE_NAME## PRIVATE "/W3")
	target_compile_options(##PACKAGE_NAME## PRIVATE "/wd5030")
	target_compile_options(##PACKAGE_NAME## PRIVATE "/Zc:wchar_t")
	target_compile_options(##PACKAGE_NAME## PRIVATE "/Zi")
	target_compile_options(##PACKAGE_NAME## PRIVATE "/Gm-")
	target_compile_options(##PACKAGE_NAME## PRIVATE "/Zc:inline")
	target_compile_options(##PACKAGE_NAME## PRIVATE "/fp:precise")
	target_compile_options(##PACKAGE_NAME## PRIVATE "/errorReport:prompt")
	target_compile_options(##PACKAGE_NAME## PRIVATE "/GF")
	target_compile_options(##PACKAGE_NAME## PRIVATE "/WX-")
	target_compile_options(##PACKAGE_NAME## PRIVATE "/Zc:forScope")
	target_compile_options(##PACKAGE_NAME## PRIVATE "/arch:AVX")
	target_compile_options(##PACKAGE_NAME## PRIVATE "/Gd")
	target_compile_options(##PACKAGE_NAME## PRIVATE "/Oy-")
	target_compile_options(##PACKAGE_NAME## PRIVATE "/MDd")
	target_compile_options(##PACKAGE_NAME## PRIVATE "/FC")
	target_compile_options(##PACKAGE_NAME## PRIVATE "/diagnostics:classic")
	target_compile_options(##PACKAGE_NAME## PRIVATE "/EHsc")
endif()



#/**************************************************************************************************************************************************************\
# *
# *  Post build operations
# *
#\**************************************************************************************************************************************************************/



##USE_QT?##
if( UNIX )
   ADD_CUSTOM_COMMAND(
      TARGET ${PROJECT_NAME}
      POST_BUILD
      COMMAND mkdir
      ARGS -p
         "${VVV_RuntimeDir}/binary/resource")
   ADD_CUSTOM_COMMAND(
      TARGET ${PROJECT_NAME}
      POST_BUILD
      COMMAND rsync
      ARGS -raum
         "--include='*.qml'"
         "--exclude='*.*'"
         "${PROJECT_SOURCE_DIR}/resource/"
         "${VVV_RuntimeDir}/binary/resource/"
      COMMENT "Copying qml files to the runtime folder...")
elseif( WIN32 )
   file(TO_NATIVE_PATH "${PROJECT_SOURCE_DIR}\\resource" RES_SOURCE_PATH)
   file(TO_NATIVE_PATH "${VVV_RuntimeDir}" RES_TARGET_PATH)
   ADD_CUSTOM_COMMAND(
      TARGET ${PROJECT_NAME}
      POST_BUILD
      COMMAND xcopy
      ARGS
         "${RES_SOURCE_PATH}\\*.qml"
         "${RES_TARGET_PATH}\\binary\\resource"
         /S /I /Y
      COMMENT "Copying qml files to the runtime folder...")
endif()
##}##
