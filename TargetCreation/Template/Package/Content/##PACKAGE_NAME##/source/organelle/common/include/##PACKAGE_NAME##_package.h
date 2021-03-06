/**************************************************************************************************************************************************************\
 **************************************************************************************************************************************************************
 **************************************************************************************************************************************************************
 *
 * File:
 *  ##PACKAGE_NAME##_package.h  
 * ------------------------------------------------------------------------------------------------------------------------------------------------------------
 * Description:
 *  This file indirectly contains the declarations of family names and guids and species names. By default, it also indirectly contains the declarations of the
 *  exceptions used in the package. This files can be included by any consumer of the package, even without using vvv_essence. In this case VVV_WITHOUT_ESSENCE
 *  must be defined before including this file.
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



#ifndef VVV_WITHOUT_ESSENCE
    #include "vvv_essence.h"
#endif



/**************************************************************************************************************************************************************\
 *
 *  Namespaces
 *
\**************************************************************************************************************************************************************/



namespace vvv::vvvpkg::##UND_PACKAGE_NAME##_package
{



/**************************************************************************************************************************************************************\
 *
 *  Global variables
 *
\**************************************************************************************************************************************************************/



#ifndef VVV_WITHOUT_ESSENCE
    //  this is the description structure of the organelle package
    extern organelle_package_description our_pkg_desc;
#endif



}  //  namespace vvv::vvvpkg::##UND_PACKAGE_NAME##_package



/**************************************************************************************************************************************************************\
 *
 *  Includes
 *
\**************************************************************************************************************************************************************/



#ifndef VVV_WITHOUT_ESSENCE
    //  this contains the definitions of the exceptions used in the package. you only need to include this if you want to check thrown exceptions for their
    //  error codes
    #include "##PACKAGE_NAME##_exception.h"
#endif
//  species names
#include "##PACKAGE_NAME##_symbiosis.h"
