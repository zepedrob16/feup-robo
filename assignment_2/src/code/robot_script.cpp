#include "robot_script.h"
using namespace std;
  
RobotScript::RobotScript(int argc,char **argv) {

	ros::NodeHandle nh;

    std::string robot_cmd_vel = std::string("/") + std::string(argv[1]) + std::string("/cmd_vel");

    std::string laser = std::string("/") + std::string(argv[1]) + std::string("/") + std::string(argv[2]);
      
    laser_subscriber = nh.subscribe(laser.c_str(), 1, &RobotScript::robotScanner, this);
      
    cmd_vel_advertiser = nh.advertise<geometry_msgs::Twist>(robot_cmd_vel.c_str(), 1);
  }
  
RobotScript::~RobotScript(void) {
    
}
  
void RobotScript::robotScanner(const sensor_msgs::LaserScan& msg) {
   
	geometry_msgs::Twist cmd;

	cmd.linear.x = 0.4;
	cmd.angular.z = 0.1;

	cmd_vel_advertiser.publish(cmd);

}
