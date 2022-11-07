/**************************************************************************************************************************************************************\
 **************************************************************************************************************************************************************
 **************************************************************************************************************************************************************
 *
 * File:
 *  moba_##SENSOR_NAME##.cpp  
 * ------------------------------------------------------------------------------------------------------------------------------------------------------------
 * Description:
 *  Implements the class moba_##SENSOR_NAME## which is the idea of a ##SENSOR_NAME## sensor.
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



#include "../header/pch.h"
#include "vvv_moba_library.h"
#include "../header/moba_##SENSOR_NAME##.h"



/**************************************************************************************************************************************************************\
 *
 *  Namespaces
 *
\**************************************************************************************************************************************************************/



namespace moba_library
{



/**************************************************************************************************************************************************************\
 *
 *  Static and global variables
 *
\**************************************************************************************************************************************************************/



const wchar*              moba_##SENSOR_NAME##::my_species = moba_symbiosis_strings::species_moba_##SENSOR_NAME##;  //  this must be defined in the symbiosis library header of the package

const organelle_functions moba_##SENSOR_NAME##::functions(
                                           organelle_class::rebus,
                                           can_thing<moba_##SENSOR_NAME##>::init_species,
                                           organelle::synthesise<moba_##SENSOR_NAME##>,
                                           can_thing<moba_##SENSOR_NAME##>::deinit_species);

const organelle_info      moba_##SENSOR_NAME##_org_info(moba_##SENSOR_NAME##::my_species, moba_##SENSOR_NAME##::functions);



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
    moba_##SENSOR_NAME##::detector::on_data_received
---------------------------------------------------------------------------------------------------------------------------------------------------------------
 Description:
    Static function that checks whether the contents of the input buffer belong to the stuff that the real sensor could send.
---------------------------------------------------------------------------------------------------------------------------------------------------------------
 Parameters:
    in_buf  [i] the input buffer. it is assumed that the buffer contains at least one entire CAN message
 Return Value:
\**************************************************************************************************************************************************************/

void moba_##SENSOR_NAME##::detector::on_data_received(buffer& in_buf)
{
    //  shortcut
    can_message& can_msg(reinterpret_cast<can_message&>(in_buf));
               
    //  we can only detect CAN messages that start right at the beginning of the buffer        
    can_msg_id_t msg_id = can_msg.get_id();
    
    //  check whether the buffer contains one of our CAN messages and disable further detection
    if (msg_id == can_msg_id_##MSG_ID_NAME##)
    {
        if (!enabled_)
            return;
        enabled_ = false;
    }
    else
    {
        return;
    }
    //  remove the CAN message
    can_msg.remove();
 
    //  ask the demiurg to asynchronously synthesise an organelle of our attached species
    driver_->get_vivid().get_demiurg().synthesise_organelle(
                                         moba_symbiosis_strings::species_moba_##SENSOR_NAME##,
                                         synthesis_reason::detected,
                                         new can_thing_detection_hint(ikwiad_ncr(driver_->get_bridge_builder()), this),
                                         nullptr);
}  //  moba_##SENSOR_NAME##::detector::on_data_received



/**************************************************************************************************************************************************************\
 **************************************************************************************************************************************************************
 **************************************************************************************************************************************************************
 *
 * class moba_##SENSOR_NAME##
 *
 **************************************************************************************************************************************************************
 **************************************************************************************************************************************************************
\**************************************************************************************************************************************************************/



/**************************************************************************************************************************************************************\
 Function:
    moba_##SENSOR_NAME##::moba_##SENSOR_NAME##
---------------------------------------------------------------------------------------------------------------------------------------------------------------
 Description:
    Constructor.
---------------------------------------------------------------------------------------------------------------------------------------------------------------
 Parameters:
    synth_info  [i] the organelle synthesis information
 Return Value:
\**************************************************************************************************************************************************************/

moba_##SENSOR_NAME##::moba_##SENSOR_NAME##(const synthesis_info& synth_info) :
    can_thing<moba_##SENSOR_NAME##>(synth_info),
    family_##FAMILY_NAME##(synth_info.my_vivid.get_agora(), guard_),
    meas_prot_(true),
    request_handler_(*this),
    max_time_without_measurement_(max_time_without_measurement)
{
}  //  moba_##SENSOR_NAME##::moba_##SENSOR_NAME##


/**************************************************************************************************************************************************************\
 Function:
    moba_##SENSOR_NAME##::get_can_thing_detector
---------------------------------------------------------------------------------------------------------------------------------------------------------------
 Description:
    Called by a CAN driver on synthesis. Creates and registers a new detector.
---------------------------------------------------------------------------------------------------------------------------------------------------------------
 Parameters:
    functionality  [i] the functionality registered by can_thing<moba_##SENSOR_NAME##>::init_species
    driver         [t] the CAN driver
 Return Value:
\**************************************************************************************************************************************************************/

void moba_##SENSOR_NAME##::get_can_thing_detector(
                      can_thing_functionality* functionality,
                      obj_ptr<can_driver>&     driver)
{
    //  create a new detector. it does not need anything from the functionality object e.g. the config cursor), therefore we don't pass it
    detector* det = new detector(driver);
    //  register the detector at the driver
    can_msg_id_t ids[] = {can_msg_id_##MSG_ID_NAME##};
    can_bus_protocol_multiplexer_info info(det, _countof(ids), ids);
    driver->register_detector(info);
}  //  moba_##SENSOR_NAME##::get_can_thing_detector


/**************************************************************************************************************************************************************\
 Function:
    moba_##SENSOR_NAME##::dissolve
---------------------------------------------------------------------------------------------------------------------------------------------------------------
 Description:
    Finishes all symbioses and releases all resources. Writes the settings to the configuration.
---------------------------------------------------------------------------------------------------------------------------------------------------------------
 Parameters:
 Return Value:
\**************************************************************************************************************************************************************/

void moba_##SENSOR_NAME##::dissolve(void)
{
    try
    {
        //  call the event reactors
        call_on_organelle_event(organelle_event::dysbiosis);

        //  symbiosis
        vivid_.get_agora().unregister_donor(guard_->get_virtual_pointer<moba_##SENSOR_NAME##, donor_internal_intf>());

        //  clear the multiplexer
        can_msg_id_t ids[] = {can_msg_id_##MSG_ID_NAME##};
        can_bus_protocol_multiplexer_info info(nullptr, _countof(ids), ids);
        clear_multiplexer(info);

        //  remove any pending requests
        our_home.get_handyman().abort(&request_handler_, infinite);

        //  finish our symbioses with the bridge and the acceptors
        dysbiotise();
        //  after having disappeared from the public eye, we begin the inner dissolution
        begin_inner_dissolution();
    }
    catch (vvvex& ex)
    {
        //  add more information to the exception        
        vvvex_organelle_dissolution::virtual_ex(&ex, my_species);
    }
    //  finish the dissolution
    finish_dissolution();
}  //  moba_##SENSOR_NAME##::dissolve


/**************************************************************************************************************************************************************\
 Function:
    moba_##SENSOR_NAME##::get_species
---------------------------------------------------------------------------------------------------------------------------------------------------------------
 Description:
    Returns the name of the organelle species.
---------------------------------------------------------------------------------------------------------------------------------------------------------------
 Parameters:
 Return Value:
    the name of the species
\**************************************************************************************************************************************************************/

const wchar* moba_##SENSOR_NAME##::get_species(void) const
{
    return my_species;
}  //  moba_##SENSOR_NAME##::get_species


/**************************************************************************************************************************************************************\
 Function:
    moba_##SENSOR_NAME##::write_genome
---------------------------------------------------------------------------------------------------------------------------------------------------------------
 Description:
    Writes the object setttings to the configuration.
---------------------------------------------------------------------------------------------------------------------------------------------------------------
 Parameters:
 Return Value:
\**************************************************************************************************************************************************************/

void moba_##SENSOR_NAME##::write_genome(void)
{
    //  write lock the configuration
    config_lock cl(cursor_, config_lock::write_locked);
    //  if the configuration cursor is not attached, we cannot write
    if (!cursor_.is_attached())
        return;
}  //  moba_##SENSOR_NAME##::write_genome


/**************************************************************************************************************************************************************\
 Function:
    moba_##SENSOR_NAME##::get_donor
---------------------------------------------------------------------------------------------------------------------------------------------------------------
 Description:
    Returns the donor interface implementation of the object.
---------------------------------------------------------------------------------------------------------------------------------------------------------------
 Parameters:
 Return Value:
    the donor interface implementation
\**************************************************************************************************************************************************************/

vtl_ptr<donor_intf> moba_##SENSOR_NAME##::get_donor(void)
{
    return guard_->get_virtual_pointer<moba_##SENSOR_NAME##, donor_intf>();
}  //  moba_##SENSOR_NAME##::get_donor


/**************************************************************************************************************************************************************\
 Function:
    moba_##SENSOR_NAME##::interpret
---------------------------------------------------------------------------------------------------------------------------------------------------------------
 Description:
    This function is called directly from the bridge. It expects CAN messages and extracts the measurement value from them. The respective mb is then set.
---------------------------------------------------------------------------------------------------------------------------------------------------------------
 Parameters:
    bridge  [i] the bridge
    in_buf  [t] the buffer that contains the input data
 Return Value:
    either input_type::asynchronous if an asynchronous message was found in the buffer or input_type::synchronous if the data in the buffer was not recognised
    as an asynchronous message
\**************************************************************************************************************************************************************/

input_type moba_##SENSOR_NAME##::interpret(
                            full_bridge& bridge,
                            buffer&      in_buf)
{
    //  shortcut
    can_message& can_msg((can_message&)in_buf);

    //  we loop as long as there are complete asynchronous messages in the input buffer
    while (true)
    {
        //  if there is no complete message
        if (can_msg.get_size() < can_message::min_len || can_msg.get_size() < can_msg.get_length())
            return input_type::asynchronous;

        //  if we get messages that are not interesting for us, remove them from the buffer
        can_msg_id_t msg_id = can_msg.get_id();
        if (msg_id != can_msg_id_##MSG_ID_NAME##)
            goto REM_MSG;

        //  we have contact with our physical counterpart
        meas_prot_.lock();
        last_measurement_time_.to_now();
        last_contact_time_.to_now();
        meas_prot_.unlock();

        //  if the sensor has not yet been measuring, change its state
        if (state_ != thing_state::measuring)
        {
            state_ = thing_state::measuring;
            call_on_thing_state_changed();
        }

        //  get the measurement state information from the message
        can_msg.set_parse_index(can_message::data1_ind);
        byte measurement_state_;
        can_msg.get(&measurement_state_, 1);
        //  if the measurement is out of range
        double value;
        if (measurement_state_ == measurement_out_of_range)
        {
            value = ##MEAS_DIM##::invalid;
        }
        //  if the measurement is ok
        else  //  if (measurement_state_ ==
        {
            //can_msg.move_parse_position(sizeof(i16));
            //  get the ##MEAS_DIM##
            //value  = ((double)((byte(can_msg)) / 360.0 * TWOPI);
        }  //  if (measurement_state_ ==

        //  set the ##MEAS_DIM## metabolic
        mbp_angle_->set(##MEAS_DIM##(value));

REM_MSG:
        //  remove the message
        can_msg.remove();
    }  //  while (true)

    return input_type::asynchronous;
}  //  moba_##SENSOR_NAME##::interpret


/**************************************************************************************************************************************************************\
 Function:
    moba_##SENSOR_NAME##::initialise
---------------------------------------------------------------------------------------------------------------------------------------------------------------
 Description:
    Reads the settings from the configuration, sets the identity of the sensor and starts symbiosis.
---------------------------------------------------------------------------------------------------------------------------------------------------------------
 Parameters:
    synth_info  [i] the organelle synthesis information
 Return Value:
\**************************************************************************************************************************************************************/

void moba_##SENSOR_NAME##::initialise(const synthesis_info& synth_info)
{
    //  lock the configuration
    config_lock cl(synth_info.cursor, config_lock::read_locked);

    //  call the base class implementation
    can_thing::initialise(synth_info, moba_##SENSOR_NAME##_strings::object_name);

    //  read the configuration (if cursor_ is not attached, we will use the hardcoded default values)
    read_genome();
    //  we do not need to lock the configuration anymore
    cl.read_unlock();

    //  now the thing is really there        
    state_ = thing_state::created;
    //  call the event reactors
    call_on_organelle_event(organelle_event::synthesis);

    //  from now on we will interpret data that is coming in over the bridge
    command_set<fam_desc_full_bridge::csi> bridge;
    if (!environment_[br_smbi]->get_command_set(br_cs_btci, bridge))
        vvvex_thing_no_bridge::throw_ex(nullptr);
    bridge->set_idea(this);
    if (!driver_.is_valid())
        driver_ = bridge->get_driver();
    if (!driver_.is_valid())
        vvvex_sys_invalid_pointer::throw_ex(nullptr);

    //  set up the multiplexer
    can_msg_id_t ids[] = {can_msg_id_##MSG_ID_NAME##};
    can_bus_protocol_multiplexer_info info(bridge, _countof(ids), ids);
    driver_->setup_multiplexer(info);

    //  the thing will periodically do some checks etc.
    last_contact_time_.to_now();
    our_home.get_handyman().lend_me_a_hand_later(&request_handler_, nullptr, com_check_interval);

    //  the thing idea is fully initialised
    state_ = thing_state::initialised;
    call_on_thing_state_changed();

    //  symbiosis           
    //  we provide angles
    vivid_.get_agora().register_donor(  
                         holo_taxon(identity_, my_species, ##FAMILY_NAME##_symbiosis_strings::family_##FAMILY_NAME##, organelle_class::rebus),
                         guard_->get_virtual_pointer<moba_##SENSOR_NAME##, donor_internal_intf>(),
                         synth_info.synth_reason != synthesis_reason::as_companion);

    //  call the event reactors
    call_on_organelle_event(organelle_event::symbiosis);
}  //  moba_##SENSOR_NAME##::initialise


/**************************************************************************************************************************************************************\
 Function:
    moba_##SENSOR_NAME##::read_genome
---------------------------------------------------------------------------------------------------------------------------------------------------------------
 Description:
    Reads the settings from the configuration.
---------------------------------------------------------------------------------------------------------------------------------------------------------------
 Parameters:
 Return Value:
\**************************************************************************************************************************************************************/

void moba_##SENSOR_NAME##::read_genome(void)
{
    //  reset the settings
//    settings_.set_to_defaults();
    //  if the configuration cursor is not attached, we cannot read
//    if (!cursor_.is_attached())
//        return;

    //  read the sensor settings
//    settings_.read(cursor_);
}  //  moba_##SENSOR_NAME##::read_genome


/**************************************************************************************************************************************************************\
 Function:
    moba_##SENSOR_NAME##::my_handle_request
---------------------------------------------------------------------------------------------------------------------------------------------------------------
 Description:
    Called by the handyman. Checks the communication state with the sensor.
---------------------------------------------------------------------------------------------------------------------------------------------------------------
 Parameters:
    req  [i] not used
 Return Value:
    moba_##SENSOR_NAME##::com_check_interval. this is the time span after which the function shall be called again
\**************************************************************************************************************************************************************/

u32 moba_##SENSOR_NAME##::my_handle_request(request* req)
{
    vvv_trace_hm_der(moba_##SENSOR_NAME##, this, req);

    //  determine the current time
    tempus now(false);

    //  if the sensor is (or recently has been) sending measurement values
    if (state_ == thing_state::measuring)
    {
        //  if the last measurement is too old
        if (now > last_measurement_time_ + tempus::from_milliseconds(max_time_without_measurement_))
        {
            //  invalidate the metabolic
            mbp_angle_->set(##MEAS_DIM##(##MEAS_DIM##::invalid));

            //  call the event reactor            
            state_ = thing_state::initialised;
            call_on_thing_state_changed();
        }  //  if (alarm_time > last_measurement_time_
    }
    //  if the sensor is not sending measurement values
    else if (state_ != thing_state::contact_lost)
    {
        //  check the time span between now and the time we had contact with the sensor for the last time
        meas_prot_.lock();
        tempus last_contact_time_copy(last_contact_time_);
        meas_prot_.unlock();
        
        if (now > last_contact_time_copy + max_time_without_contact)
        {
            vvv_trace_3(log_pkg_moba_ext, L"Lost contact to thing '%ls'", identity_.get_name().get_data());
            state_ = thing_state::contact_lost;
            call_on_thing_state_changed();
        }  //  if (alarm_time >
    }  //  if (state_

    //  we want to be called again
    return com_check_interval;
}  //  moba_##SENSOR_NAME##::my_handle_request


/**************************************************************************************************************************************************************\
 Function:
    moba_##SENSOR_NAME##::calibrate
---------------------------------------------------------------------------------------------------------------------------------------------------------------
 Description:
    Sets the offset of the sensor.
---------------------------------------------------------------------------------------------------------------------------------------------------------------
 Parameters:
    new_value  [i] the measurement value which the sensor shall provide for the actual current reading
 Return Value:
\**************************************************************************************************************************************************************/

void moba_##SENSOR_NAME##::calibrate(##MEAS_DIM## new_value)
{
}  //  moba_##SENSOR_NAME##::calibrate



}  //  namespace moba_library
