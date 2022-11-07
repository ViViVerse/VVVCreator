/**************************************************************************************************************************************************************\
 **************************************************************************************************************************************************************
 **************************************************************************************************************************************************************
 *
 * File:
 *  moba_##SENSOR_NAME##.h  
 * ------------------------------------------------------------------------------------------------------------------------------------------------------------
 * Description:
 *  Declares the class moba_##SENSOR_NAME## which is the idea of a ##SENSOR_NAME## sensor.
 * ------------------------------------------------------------------------------------------------------------------------------------------------------------
 * Created:
 *  ##DATE##
 * ------------------------------------------------------------------------------------------------------------------------------------------------------------
 * Copyright (c):
 *  Moba AG  
 *
 **************************************************************************************************************************************************************
 **************************************************************************************************************************************************************
\**************************************************************************************************************************************************************/



/**************************************************************************************************************************************************************\
 *
 *  Includes
 *
\**************************************************************************************************************************************************************/



#include "moba.h"



/**************************************************************************************************************************************************************\
 *
 *  Namespace
 *
\**************************************************************************************************************************************************************/



namespace moba_library
{



/**************************************************************************************************************************************************************\
 *
 *  String constants
 *
\**************************************************************************************************************************************************************/



namespace moba_##SENSOR_NAME##_strings
{
    //  ##SENSOR_NAME## object
    inline const wchar object_name[] = L"%ls-##OBJECT_NAME##";
    //  configuration
}  //  namespace moba_##SENSOR_NAME##_strings



/**************************************************************************************************************************************************************\
 *
 *  Class declarations
 *
\**************************************************************************************************************************************************************/



/**************************************************************************************************************************************************************\
 Class:
    moba_##SENSOR_NAME##
---------------------------------------------------------------------------------------------------------------------------------------------------------------
 Description:
    A ##SENSOR_NAME## sensor.
\**************************************************************************************************************************************************************/

class moba_##SENSOR_NAME## :
    public can_thing<moba_##SENSOR_NAME##>,
    public donor<moba_##SENSOR_NAME##, family_##FAMILY_NAME##>,
    public family_##FAMILY_NAME##
{
//  nested classes
    class detector :
        public can_detector
    {
    //  construction, destruction
    public:
        detector(obj_ptr<can_driver>& driver);
    
    //  operations
    public:
        //  in_data_handler_intf implementation
        void on_data_received(buffer& in_buf) override;

    //  data members
    protected:
    };  //  class detector

//  friends
    friend class organelle;
    friend don_base_templ;
    friend class request_handler<moba_##SENSOR_NAME##>;

//  construction, destruction
protected:
    moba_##SENSOR_NAME##(const synthesis_info& synth_info);

//  static functions:
public:    
    static void get_can_thing_detector(
                  can_thing_functionality* functionality,
                  obj_ptr<can_driver>&     driver);

//  operations
public:
    //  organelle implementation
    void dissolve(void) override;
    const wchar* get_species(void) const override;
    void write_genome(void) override;
    //  family implementation
    vtl_ptr<donor_intf> get_donor(void) override;
    
    //  idea_intf implementation
    input_type interpret(
                 full_bridge& bridge,
                 buffer&      in_buf) override;

protected:
    //  helpers
    void initialise(const synthesis_info& synth_info);
    void read_genome(void);

//  interface implementations
protected:                                                  
    //  request_handler_intf (indirectly via request_handler)
    u32 my_handle_request(request* req);
    //  family_##FAMILY_NAME##::csi
    void calibrate(##MEAS_DIM## new_##SENSOR_NAME##) override;

//  constants
    static constexpr u32 can_msg_id_##MSG_ID_NAME## = ##CAN_MSG_ID##;                   //  the possible messages coming from the sensor

    static constexpr u32 can_msg_id_module_config = 0x07EC;                 //  for configuring the sensor
    static constexpr u32 can_msg_id_module_status = 0x07ED;                 //  sensor status message

    static constexpr byte measurement_out_of_range = 0x00005;               //  measurement state

    static constexpr u32 com_check_interval           = 1000;               //  [ms] the interval in which the communication with the real sensor is checked
    static constexpr u32 max_time_without_measurement = 1000;               //  [ms] after this time without new measurements the thing goes back from measu-
                                                                            //  ring to intialised state
//  data members
public:
    static const wchar*              my_species;                            //  the species name

    static const organelle_functions functions;                             //  the functions necessary for building an organelle module

protected:
    tempus                         last_measurement_time_,                  //  the time when the last measurement came in
                                   last_contact_time_;                      //  the time when we last heard from the physical device (measurement or other)
             
    u32                            max_time_without_measurement_;           //  [ms] the maximum time that the 'measuring' state will be kept after the last
                                                                            //  measurement
    protect                        meas_prot_;                              //  makes access to the measurements thread safe

    request_handler<moba_##SENSOR_NAME##> request_handler_;                        //  we need the helper here because we already derive from family and thus from
                                                                            //  request_handler_intf. deriving again is not liked by the c++ standard
};  //  class moba_##SENSOR_NAME##



/**************************************************************************************************************************************************************\
 *
 *  Implementation
 *
\**************************************************************************************************************************************************************/



/**************************************************************************************************************************************************************\
 **************************************************************************************************************************************************************
 **************************************************************************************************************************************************************
 *
 * class moba_##SENSOR_NAME##::detector
 *
 **************************************************************************************************************************************************************
 **************************************************************************************************************************************************************
\**************************************************************************************************************************************************************/



/**************************************************************************************************************************************************************\
 Function:
    moba_##SENSOR_NAME##::detector::detector
---------------------------------------------------------------------------------------------------------------------------------------------------------------
 Description:
    Constructor.
---------------------------------------------------------------------------------------------------------------------------------------------------------------
 Parameters:
    driver  [i] the CAN driver to which the detector is connected
 Return Value:
\**************************************************************************************************************************************************************/

inline moba_##SENSOR_NAME##::detector::detector(obj_ptr<can_driver>& driver) :
    can_detector(driver)
{
}  //  moba_##SENSOR_NAME##::detector::detector



}  //  namespace moba_library
