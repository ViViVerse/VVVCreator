/**************************************************************************************************************************************************************\
 **************************************************************************************************************************************************************
 **************************************************************************************************************************************************************
 *
 * File:
 *  vvv_##FAMILY_NAME##_symbiosis.h  
 * ------------------------------------------------------------------------------------------------------------------------------------------------------------
 * Description:
 *  Contains the declarations of the family ##FAMILY_NAME##. This family provides functionality to .
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
 *  Namespace
 *
\**************************************************************************************************************************************************************/



namespace vvv::vvvpkg
{
inline namespace ##PACKAGE_NAME##
{



/**************************************************************************************************************************************************************\
 *
 *  String constants
 *
\**************************************************************************************************************************************************************/



namespace ##FAMILY_NAME##_symbiosis_strings
{
    //  families
    inline const wchar family_##FAMILY_NAME##[] = L"##dus(FAMILY_NAME##";
}  //  namespace ##FAMILY_NAME##_symbiosis_strings



/**************************************************************************************************************************************************************\
 *
 *  Aliases
 *
\**************************************************************************************************************************************************************/



/*-------------------------------------------------------------------------------------------------------------------------------------------------------------+
|  metabolics
+-------------------------------------------------------------------------------------------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------------------------------------------------------------------------------------------+
|  genes
+-------------------------------------------------------------------------------------------------------------------------------------------------------------*/



/**************************************************************************************************************************************************************\
 *
 *  Families
 *
\**************************************************************************************************************************************************************/



/**************************************************************************************************************************************************************\
 Class:
    fam_desc_##FAMILY_NAME##
---------------------------------------------------------------------------------------------------------------------------------------------------------------
 Description:
    Describes the family of the ##FAMILY_NAME## objects.
\**************************************************************************************************************************************************************/

class fam_desc_##FAMILY_NAME## : public viviversal_family_description<
                               fam_desc_##FAMILY_NAME##,
                               ##FAMILY_NAME##_symbiosis_strings::family_##FAMILY_NAME##,
                               make_guid(##CODE_GUID##)>
{
//  friends
    friend base_templ;

//  command set
public:
    interface csi : public command_set_intf
    {
##HAS_SYNCHRONOUS_COMMANDS?##
        virtual i32 sync_example(i32 param) = 0;
##}##
##HAS_ASYNCHRONOUS_COMMANDS?##
        virtual void async_example(const async_exec_cont& asex_cont) = 0;
##}##
    };  //  interface csi
##HAS_SYNCHRONOUS_COMMANDS?##
##HAS_ASYNCHRONOUS_COMMANDS?##
    fam_desc_cs_decl(cmd_sync_example, cmd_async_example)
##}##
##!HAS_ASYNCHRONOUS_COMMANDS?##
    fam_desc_cs_decl(cmd_sync_example)
##}##
##}##
##!HAS_SYNCHRONOUS_COMMANDS?##
##HAS_ASYNCHRONOUS_COMMANDS?##
    fam_desc_cs_decl(cmd_async_example)
##}##
##}##
    
//  metabolics, genes
public:
##HAS_METABOLICS?##
    fam_desc_mb(mb_example)
##}##
##HAS_GENES?##
    fam_desc_gn(gn_example)
##}##
};  //  class fam_desc_##FAMILY_NAME##


/**************************************************************************************************************************************************************\
 Class:
    family_##FAMILY_NAME##
---------------------------------------------------------------------------------------------------------------------------------------------------------------
 Description:
    The family of the ##FAMILY_NAME##s.
\**************************************************************************************************************************************************************/

class family_##FAMILY_NAME## : public fam_templ<
                             fam_desc_##FAMILY_NAME##,
                             family_##FAMILY_NAME##>
{
//  friends
    friend base_templ;

//  construction, destruction
public:
    family_##FAMILY_NAME##(
      agora&      ag,
##!HAS_GENES?##
      guard_intf* member) : base_templ(member)
##}##
##HAS_GENES?##
      guard_intf* member,
      i32*        example) : base_templ(member)
##}##
##HAS_METABOLICS?##
                            , mbp_example_(ag, this, 0)
##}##
##HAS_GENES?##
                            , gnp_example_(ag, this, false, example)
##}##
    {
    }

//  base class and interface implementations
protected:
##HAS_COMMANDS?##
    virtual bool execute_command_set_command(
                   cs_cmd&          command,
                   buffer*          cmd_buf,
                   buffer*          res_buf,
                   command_request* cmd_req) override;
##}##

##HAS_ASYNCHRONOUS_COMMANDS?##
//  partial standard implementation of asynchronous commands
public:
    virtual void async_example(const async_exec_cont& asex_cont) override;
##}##

##HAS_ASYNCHRONOUS_COMMANDS?##
protected:
    //  these are the synchronous implementations of the asynchronous functions. must be implemented by each family member
    virtual void async_example(ref_ptr<async_exec>& asex) = 0;
##}##

//  data members
protected:
##HAS_METABOLICS?##
    mb_ptr<i32> mbp_example_;                                               //  the example metabolic
##}##

##HAS_GENES?##
    gn_ptr<i32> gnp_example_;                                               //  the example gene
##}##
};  //  class family_##FAMILY_NAME##


/**************************************************************************************************************************************************************\
 Class:
    family_idea_##FAMILY_NAME##
---------------------------------------------------------------------------------------------------------------------------------------------------------------
 Description:
    The family idea of the ##FAMILY_NAME##s.
\**************************************************************************************************************************************************************/

class family_idea_##FAMILY_NAME## : public family_idea<
                                  fam_desc_##FAMILY_NAME##,
                                  family_idea_##FAMILY_NAME##>
{
//  friends
    friend base_templ;

//  construction, destruction
public:
    family_idea_##FAMILY_NAME##(
      agora&                              ag,
      const vtl_ptr<donor_internal_intf>& don,
      const obj_ptr<dendron>&             den,
      donid_t                             did) : base_templ(don, den, did)
##HAS_METABOLICS?##
                                                 , mbp_example_(ag, this, 0)
##}##
##HAS_GENES?##
                                                 , gnp_example_(ag, this, false)
##}##
    {
    }

//  base class and interface implementations
protected:
    //  fam_desc_##FAMILY_NAME##::csi
##HAS_SYNCHRONOUS_COMMANDS?##
    virtual i32 sync_example(i32 param) override;
##}##
##HAS_ASYNCHRONOUS_COMMANDS?##
    virtual void async_example(const async_exec_cont& asex_cont) override;
##}##

//  data members
protected:
##HAS_METABOLICS?##
    mb_ptr<i32> mbp_example_;                                               //  the example metabolic
##}##

##HAS_GENES?##
    ecto_gn_ptr<i32> gnp_example_;                                          //  the example gene
##}##
};  //  class family_idea_##FAMILY_NAME##



}  //  inline namespace ##PACKAGE_NAME##
}  //  namespace vvv::vvvpkg
