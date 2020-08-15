/**************************************************************************************************************************************************************\
 **************************************************************************************************************************************************************
 **************************************************************************************************************************************************************
 *
 * File:
 *  ##FAMILY_NAME##_symbiosis.cpp  
 * ------------------------------------------------------------------------------------------------------------------------------------------------------------
 * Description:
 *  Provides the acceptor and donor side proxy implementations of all command_set_intf derived interfaces in the module.
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
 *  Includes
 *
\**************************************************************************************************************************************************************/



#include "../header/pch.h"
#include "vvv_##FAMILY_NAME##_symbiosis.h"



/**************************************************************************************************************************************************************\
 *
 *  Namespace
 *
\**************************************************************************************************************************************************************/



namespace vvv::vvvpkg::##PACKAGE_NAME##
{



/**************************************************************************************************************************************************************\
 *
 *  Implementation
 *
\**************************************************************************************************************************************************************/



/**************************************************************************************************************************************************************\
 **************************************************************************************************************************************************************
 **************************************************************************************************************************************************************
 *
 * class family_##FAMILY_NAME##
 *
 **************************************************************************************************************************************************************
 **************************************************************************************************************************************************************
\**************************************************************************************************************************************************************/



##HAS_COMMANDS?##
/**************************************************************************************************************************************************************\
 Function:
    family_##FAMILY_NAME##::execute_command_set_command    
---------------------------------------------------------------------------------------------------------------------------------------------------------------
 Description:
    Synchronously executes the given command. If the command is asynchronous, this function is called within a work thread in the standard implementation (see
    family::initiate_command_execution). The base family implementation should be called first and the function should return true if the base family has hand-
    led the command.
---------------------------------------------------------------------------------------------------------------------------------------------------------------
 Parameters:
    command  [i] contains the command set command id and timeout (for synchronous commands)
    cmd_buf  [i] contains the command parameters
    res_buf  [o] on exit receives the return value and the output parameters of the command
    cmd_req  [i] contains the command request. needed for asynchronous commands
 Return Value:
    true if the command is known. if false, the request result will be set to command_state::invalid_command_id
\**************************************************************************************************************************************************************/

bool family_##FAMILY_NAME##::execute_command_set_command(
                    cs_cmd&          command,
                    buffer*          cmd_buf,
                    buffer*          res_buf,
                    command_request* cmd_req)
{
    //  execute the given command
    switch (command.id)
    {
##HAS_SYNCHRONOUS_COMMANDS?##
        case fam_desc_##FAMILY_NAME##::cmd_sync_example:    //  sync
            {
                //  extract the input parameters from the command buffer
                i32 param(*cmd_buf);
                //  call the 'real' implementation
                i32 ret = sync_example(param);
                //  append the return value and the output parameters to the result buffer
                *res_buf << ret;
            }
            break;
##}##
##HAS_ASYNCHRONOUS_COMMANDS?##

        case fam_desc_##FAMILY_NAME##::cmd_async_example:   //  async
            {
                //  call the 'real' implementation
                async_example(((async_command_request*)cmd_req)->asex);
            }
            break;
##}##

        default:
            //  the command id is not known
            return false;
    }  //  switch (command.id

    //  the command id is known
    return true;
}  //  family_##FAMILY_NAME##::execute_command_set_command
##}##


##HAS_ASYNCHRONOUS_COMMANDS?##
/**************************************************************************************************************************************************************\
 Function:
    family_idea_##FAMILY_NAME##::async_example    
---------------------------------------------------------------------------------------------------------------------------------------------------------------
 Description:
    .
---------------------------------------------------------------------------------------------------------------------------------------------------------------
 Parameters:
    asex_cont  [t] receives or contains the asynchronous execution structure. see async_exec_cont for more information. the respective member of this structure
                   is updated prior to sending the command. thus if an intermediate or final reply arrives before the function has returned, the structure will
                   already be known to the caller
 Return Value:
\**************************************************************************************************************************************************************/

void family_##FAMILY_NAME##::async_example(const async_exec_cont& asex_cont)
{
    //  move the command id and parameters into a buffer
    async_cmd_buf cmd_buf(fam_desc_##FAMILY_NAME##::cmd_async_example);

    //  initiate the asynchronous execution
    initiate_command_execution(cmd_buf, asex_cont);
}  //  family_##FAMILY_NAME##::async_example
##}##



/**************************************************************************************************************************************************************\
 **************************************************************************************************************************************************************
 **************************************************************************************************************************************************************
 *
 * class family_idea_##FAMILY_NAME##
 *
 **************************************************************************************************************************************************************
 **************************************************************************************************************************************************************
\**************************************************************************************************************************************************************/



##HAS_SYNCHRONOUS_COMMANDS?##
/**************************************************************************************************************************************************************\
 Function:
    family_##FAMILY_NAME##::sync_example
---------------------------------------------------------------------------------------------------------------------------------------------------------------
 Description:
    .
---------------------------------------------------------------------------------------------------------------------------------------------------------------
 Parameters:
    param  [i] the parameter
 Return Value:
    the return value
\**************************************************************************************************************************************************************/

i32 family_##FAMILY_NAME##::sync_example(i32 param)
{
    //  compose the command message
	sync_cmd_whm cmd_whm(fam_desc_##FAMILY_NAME##::cmd_sync_example);
    cmd_whm << param;
    
    //  send the command and wait for the result
    fixed_size_whm<whisper_result> res_whm;
    fire_sync_command(cmd_whm, &res_whm);
    
    //  extract and return the return value
    return i32(res_whm);
}  //  family_##FAMILY_NAME##::sync_example


##}##
##HAS_ASYNCHRONOUS_COMMANDS?##
/**************************************************************************************************************************************************************\
 Function:
    family_idea_##FAMILY_NAME##::async_example    
---------------------------------------------------------------------------------------------------------------------------------------------------------------
 Description:
    .
---------------------------------------------------------------------------------------------------------------------------------------------------------------
 Parameters:
    asex_cont  [t] receives or contains the asynchronous execution structure. see async_exec_cont for more information. the respective member of this structure
                   is updated prior to sending the command. thus if an intermediate or final reply arrives before the function has returned, the structure will
                   already be known to the caller
 Return Value:
\**************************************************************************************************************************************************************/

void family_idea_##FAMILY_NAME##::async_example(const async_exec_cont& asex_cont)
{
    //  compose the command message
    async_cmd_whm cmd_whm(fam_desc_##FAMILY_NAME##::cmd_async_example);

    //  send the command and wait for the result
    fire_async_command(cmd_whm, asex_cont);
}  //  family_idea_##FAMILY_NAME##::async_example
##}##



}  //  namespace vvv::vvvpkg::##PACKAGE_NAME##
