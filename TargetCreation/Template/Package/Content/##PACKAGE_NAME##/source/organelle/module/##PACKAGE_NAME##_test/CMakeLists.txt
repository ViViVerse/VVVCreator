#/**************************************************************************************************************************************************************\
# *
# *  General
# *
#\**************************************************************************************************************************************************************/



project(vvv_##PACKAGE_NAME##_test)



#/**************************************************************************************************************************************************************\
# *
# *  Build target
# *
#\**************************************************************************************************************************************************************/



enable_testing()


find_package(GTest REQUIRED)

if(UNIX)
    add_executable(
        vvv_##PACKAGE_NAME##_test
        source/main.cpp
    )
elseif(WIN32)
    add_executable(
        vvv_##PACKAGE_NAME##_test
        source/main.cpp
    )
endif()



#/**************************************************************************************************************************************************************\
# *
# *  Output properties
# *
#\**************************************************************************************************************************************************************/



set_target_properties(vvv_##PACKAGE_NAME##_test PROPERTIES RUNTIME_OUTPUT_DIRECTORY ${VVV_BinaryDir}/${VVV_BuildPlatformDir}/${VVV_BuildConfigDir})



#/**************************************************************************************************************************************************************\
# *
# *  Preprocessor definitions, compiler and linker flags
# *
#\**************************************************************************************************************************************************************/



target_compile_options(vvv_##PACKAGE_NAME##_test PRIVATE ${VVV_AllCompilerFlags})
target_include_directories(vvv_##PACKAGE_NAME##_test PRIVATE ${GTEST_INCLUDE_DIRS})
target_link_options(vvv_##PACKAGE_NAME##_test PRIVATE ${VVV_AllLinkerFlags})
if("${CMAKE_CXX_COMPILER_ID}" STREQUAL "GNU")
    target_compile_definitions(vvv_##PACKAGE_NAME##_test PRIVATE VVV_GOOGLE_TEST)
    target_link_libraries(
        vvv_##PACKAGE_NAME##_test
        -Wl,--start-group
        ${GTEST_LIBRARIES}
        vvv_quanta
        vvv_core_symbiosis
        vvv_essence
        vvv_protein
        vvv_mathematics
        -Wl,--end-group
    )
elseif("${CMAKE_CXX_COMPILER_ID}" STREQUAL "MSVC")
# see https://stackoverflow.com/questions/46025439/unresolved-external-symbol-error-with-google-mock-and-vcpkg
    target_compile_definitions(vvv_##PACKAGE_NAME##_test PRIVATE GTEST_LINKED_AS_SHARED_LIBRARY VVV_GOOGLE_TEST)
    target_link_libraries(
        vvv_##PACKAGE_NAME##_test
        user32
        uuid
        ${GTEST_LIBRARIES}
        vvv_quanta
        vvv_essence
        vvv_protein
        vvv_mathematics
    )
endif()



#/**************************************************************************************************************************************************************\
# *
# *  Post build operations
# *
#\**************************************************************************************************************************************************************/



add_test(NAME vvv_##PACKAGE_NAME##_test COMMAND vvv_##PACKAGE_NAME##_test)
