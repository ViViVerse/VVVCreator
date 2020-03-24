#/**************************************************************************************************************************************************************/
# *
# *  Compile messages
# *
#/**************************************************************************************************************************************************************/



project(##PACKAGE_NAME##_message)
add_custom_target(${PROJECT_NAME} ALL)
add_dependencies(${PROJECT_NAME} vvv_message_compiler)

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
