project(##PACKAGE_NAME##_dna)

add_custom_target(${PROJECT_NAME} ALL)

if( UNIX )
   ADD_CUSTOM_COMMAND(
      TARGET ${PROJECT_NAME}
      POST_BUILD
      COMMAND mkdir
      ARGS -p
         "${VVV_RuntimeDir}/configuration/##PACKAGE_NAME##")
   ADD_CUSTOM_COMMAND(
      TARGET ${PROJECT_NAME}
      POST_BUILD
      COMMAND rsync
      ARGS -raum
         "--include='*.vvvdna'"
         "--include='*.xml'"
         "--exclude='*.*'"
         "${PROJECT_SOURCE_DIR}/"
         "${VVV_RuntimeDir}/configuration/##PACKAGE_NAME##"
      COMMENT "Copying package dna and xml files to the runtime folder...")
elseif( WIN32 )
   file(TO_NATIVE_PATH "${PROJECT_SOURCE_DIR}" DNA_SOURCE_PATH)
   file(TO_NATIVE_PATH "${VVV_RuntimeDir}" DNA_TARGET_PATH)
   ADD_CUSTOM_COMMAND(
      TARGET ${PROJECT_NAME}
      POST_BUILD
      COMMAND xcopy
      ARGS
         "${DNA_SOURCE_PATH}\\*.vvvdna"
         "${DNA_TARGET_PATH}\\configuration\\##PACKAGE_NAME##"
         /S /I /Y
      COMMENT "Copying package dna files to the runtime folder...")
   ADD_CUSTOM_COMMAND(
      TARGET ${PROJECT_NAME}
      POST_BUILD
      COMMAND xcopy
      ARGS
         "${PKG_SOURCE_PATH}\\*.xml"
         "${PKG_TARGET_PATH}\\configuration\\##PACKAGE_NAME##"
         /S /I /Y
      COMMENT "Copying xml files to the runtime folder...")
endif()
