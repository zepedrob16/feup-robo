#ifndef STDR_LINE_FOLLOWING
#define STDR_LINE_FOLLOWING

#include <iostream>
#include <cstdlib>
#include <cmath>

#include <ros/package.h>
#include "ros/ros.h"

#include <stdr_msgs/RobotIndexedVectorMsg.h>

#include <geometry_msgs/Pose2D.h>
#include <geometry_msgs/Point.h>
#include <geometry_msgs/Twist.h>
#include <sensor_msgs/LaserScan.h>
#include <sensor_msgs/Range.h>

class RobotScript
  {
    private:
    	ros::Publisher cmd_vel_advertiser;
    	ros::Subscriber laser_subscriber;
    	#define PI 3.14159265358979323846;

    	enum State {
    		FIND_WALL, // the robot is attempting to find its first wall
    		FOLLOWING_WALL, //the robot is following a wall
    		TURNING_RIGHT, //the robot is turning right
    		TURNING_LEFT //the robot is turning left
    	};
      	
      	State state;
      	float min_angle;
    public:
    
    //constructor
    RobotScript(int argc,char **argv);
      
    //destructor
    ~RobotScript(void);
     
    //main function
    void robotScanner(const sensor_msgs::LaserScan& msg);

    //function that turns radians to degrees
    float radians_to_degrees(float angle);
      
  };

#endif
