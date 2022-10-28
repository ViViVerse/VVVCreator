#/**************************************************************************************************************************************************************\
# *
# *  General
# *
#\**************************************************************************************************************************************************************/



project(##PACKAGE_NAME##_message)



#/**************************************************************************************************************************************************************/
# *
# *  Compile messages
# *
#/**************************************************************************************************************************************************************/



add_custom_target(${PROJECT_NAME} ALL)
# if the build is done from the umbrella project and not the package project and if we are not cross compiling, we need the message compiler of the framework
if(NOT "${CMAKE_PROJECT_NAME}" STREQUAL "${VVV_PackageName}_package" AND NOT CMAKE_CROSSCOMPILING)
    add_dependencies(${PROJECT_NAME} vvv_message_compiler)
endif()

ADD_CUSTOM_COMMAND(
    TARGET ${PROJECT_NAME}
    POST_BUILD
    COMMAND ${VVV_ToolDir}/vvv_message_compiler
    ARGS
        ${VVV_CoreDir}/include/vvv_version.h
        ${VVV_ResourceDir}
        ${CMAKE_CURRENT_SOURCE_DIR}/##PACKAGE_NAME##_message_de.vvvmp
        ${CMAKE_CURRENT_SOURCE_DIR}/##PACKAGE_NAME##_message_en.vvvmp
    COMMENT "Compiling message files...")
