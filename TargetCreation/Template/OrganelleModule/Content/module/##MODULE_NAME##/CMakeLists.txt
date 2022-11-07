#/**************************************************************************************************************************************************************\
# *
# *  General
# *
#\**************************************************************************************************************************************************************/



project(##MODULE_NAME##_mod)



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



#/**************************************************************************************************************************************************************\
# *
# *  Output properties
# *
#\**************************************************************************************************************************************************************/



set_target_properties(##MODULE_NAME##_mod PROPERTIES PREFIX "")
set_target_properties(##MODULE_NAME##_mod PROPERTIES OUTPUT_NAME "##MODULE_NAME##")
set_target_properties(##MODULE_NAME##_mod PROPERTIES SUFFIX ".vvvorg")
set_target_properties(
	##MODULE_NAME##_mod
	PROPERTIES
	ARCHIVE_OUTPUT_DIRECTORY ${VVV_BinaryDir}/${VVV_BuildPlatformDir}/${VVV_BuildConfigDir}
	LIBRARY_OUTPUT_DIRECTORY ${VVV_BinaryDir}/${VVV_BuildPlatformDir}/${VVV_BuildConfigDir}
	RUNTIME_OUTPUT_DIRECTORY ${VVV_BinaryDir}/${VVV_BuildPlatformDir}/${VVV_BuildConfigDir}
	)



#/**************************************************************************************************************************************************************\
# *
# *  Linked libraries
# *
#\**************************************************************************************************************************************************************/



if( UNIX )
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
elseif( WIN32 )
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
endif()



#/**************************************************************************************************************************************************************\
# *
# *  Preprocessor definitions, compiler and linker flags
# *
#\**************************************************************************************************************************************************************/



target_compile_options(##MODULE_NAME##_mod PRIVATE ${VVV_AllCompilerFlags})
target_link_options(##MODULE_NAME##_mod PRIVATE ${VVV_AllLinkerFlags})
if(${CMAKE_CXX_COMPILER_ID} STREQUAL "MSVC")
    target_link_options(##MODULE_NAME##_mod PRIVATE ${VVV_CoreLibDir}/vvv_face_w32.${RES_EXT})
	target_link_options(##MODULE_NAME##_mod PRIVATE ${VVV_CoreLibDir}/vvv_visual_w32.${RES_EXT})
endif()