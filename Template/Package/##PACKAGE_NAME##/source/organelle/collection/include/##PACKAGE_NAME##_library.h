/**************************************************************************************************************************************************************\
 **************************************************************************************************************************************************************
 **************************************************************************************************************************************************************
 *
 * File:
 *  ##PACKAGE_NAME##_library.h
 * ------------------------------------------------------------------------------------------------------------------------------------------------------------
 * Description:
 *  Declares the organelle information structures of all implemented organelles. This file needs to be included by organelle modules which want to use these
 *  organelles.
 * ------------------------------------------------------------------------------------------------------------------------------------------------------------
 * Created:
 *  ##CREATION_DATE##
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



#ifndef _VVV_ORGANELLE_LIB_BUILD
    #include "vvv_essence.h"
    using namespace vvv;
#endif  //  #ifndef _VVV_ORGANELLE_LIB_BUILD



/**************************************************************************************************************************************************************\
 *
 *  Organelles
 *
\**************************************************************************************************************************************************************/



//  this contains all the organelles in the collection. the organelle information structures declared below can be referenced in the organelle modules
namespace moba_library
{
//    extern const organelle_info xxx_info;
}  //  namespace moba_library
