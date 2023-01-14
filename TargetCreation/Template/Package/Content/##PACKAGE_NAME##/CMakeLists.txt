#/**************************************************************************************************************************************************************\
# *
# *  Minimum required CMake version
# *
#\**************************************************************************************************************************************************************/



cmake_minimum_required(VERSION 3.18)



#/**************************************************************************************************************************************************************\
# *
# *  Project name
# *
#\**************************************************************************************************************************************************************/



set(VVV_PackageName ##PACKAGE_NAME##)
project(${VVV_PackageName}_package)
message("******************************************* ##UND_PACKAGE_NAME## package *******************************************")



#/**************************************************************************************************************************************************************\
# *
# *  Build types
# *
#\**************************************************************************************************************************************************************/



if(NOT DEFINED CMAKE_BUILD_TYPE AND NOT DEFINED CMAKE_CONFIGURATION_TYPES)
	message(STATUS "Setting build type to 'Release' as none was specified.")
	set(CMAKE_BUILD_TYPE Release CACHE STRING "Choose the type of build." FORCE)
	set_property(CACHE CMAKE_BUILD_TYPE PROPERTY STRINGS "Debug" "Release" "ReleaseNoFace")
endif()



#/**************************************************************************************************************************************************************\
# *
# *  Special folders
# *
#\**************************************************************************************************************************************************************/



# unless already set in the overall CMakeLists.txt file, set the ViViVerse root directory
if(NOT DEFINED VVV_ViViVerseDir)
    set(VVV_ViViVerseDir ${CMAKE_CURRENT_SOURCE_DIR}/../../..)
    message("VVV_ViViVerseDir: ${VVV_ViViVerseDir}")
endif()



#/**************************************************************************************************************************************************************\
# *
# *  Includes
# *
#\**************************************************************************************************************************************************************/



include(${VVV_ViViVerseDir}/software/framework/vvv_framework/source/property/vvv_all.props.cmake)
include(${VVV_PropertyDir}/vvv_package.props.cmake)



#/**************************************************************************************************************************************************************\
# *
# *  Package contents
# *
#\**************************************************************************************************************************************************************/



add_subdirectory(source/configuration/##PACKAGE_NAME##_dna)
add_subdirectory(source/organelle/collection/##PACKAGE_NAME##)
add_subdirectory(source/organelle/common/##PACKAGE_NAME##_symbiosis)

add_subdirectory(source/organelle/module/##PACKAGE_NAME##)

add_subdirectory(source/organelle/common/##PACKAGE_NAME##_message)
