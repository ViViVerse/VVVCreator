/**************************************************************************************************************************************************************\
 **************************************************************************************************************************************************************
 **************************************************************************************************************************************************************
 *
 * File:
 *  main.cpp
 * ------------------------------------------------------------------------------------------------------------------------------------------------------------
 * Description:
 *  Implements the main function for the test.
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
 *  Includes
 *
\**************************************************************************************************************************************************************/



#include "../header/pch.h"



/**************************************************************************************************************************************************************\
 *
 *  Implementation
 *
\**************************************************************************************************************************************************************/


int main(
      int    argc,
      char** argv)
{
    ::testing::InitGoogleTest(&argc, argv);
    return RUN_ALL_TESTS();
}  //  main