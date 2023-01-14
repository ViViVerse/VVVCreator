/**************************************************************************************************************************************************************\
 **************************************************************************************************************************************************************
 **************************************************************************************************************************************************************
 *
 * File:
 *  ##LIBRARY_NAME##.h
 * ------------------------------------------------------------------------------------------------------------------------------------------------------------
 * Description:
 *  Declares the class ...
 * ------------------------------------------------------------------------------------------------------------------------------------------------------------
 * Created:
 *  ##DATE##
 * ------------------------------------------------------------------------------------------------------------------------------------------------------------
 * Copyright (c):
 *  ##COPYRIGHT##
 *
 **************************************************************************************************************************************************************
 **************************************************************************************************************************************************************
\**************************************************************************************************************************************************************/



/**************************************************************************************************************************************************************\
 *
 *  Pragmas
 *
\**************************************************************************************************************************************************************/



#pragma once



/**************************************************************************************************************************************************************\
 *
 *  Includes
 *
\**************************************************************************************************************************************************************/



//  this is only for consumers of the ##LIBRARY_NAME## library
#ifndef _##LIBRARY_NAME##_LIB_BUILD
    #include "vvv_protein.h"
#endif  //  #ifdef _##LIBRARY_NAME##_LIB_BUILD



/**************************************************************************************************************************************************************\
 *
 *  Namespace
 *
\**************************************************************************************************************************************************************/



namespace vvv::vvvpkg
{
inline namespace ##PACKAGE_NAME##
{
inline namespace ##LIBRARY_NAME##
{



/**************************************************************************************************************************************************************\
 *
 *  String constants
 *
\**************************************************************************************************************************************************************/



namespace ##UND_LIBRARY_NAME##_strings
{
}  //  namespace ##UND_LIBRARY_NAME##_strings



/**************************************************************************************************************************************************************\
 *
 *  Structure declarations
 *
\**************************************************************************************************************************************************************/



/**************************************************************************************************************************************************************\
 *
 *  Class declarations
 *
\**************************************************************************************************************************************************************/



/**************************************************************************************************************************************************************\
 *
 *  Includes
 *
\**************************************************************************************************************************************************************/



//#include "##LIBRARY_NAME##_inline.h"
//#include "##LIBRARY_NAME##_template.h"



}  //  inline namespace ##LIBRARY_NAME##
}  //  inline namespace ##PACKAGE_NAME##
}  //  namespace vvv::vvvpkg