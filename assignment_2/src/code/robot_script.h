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
    	int foundWall;

    	enum State {
    		NONE,
    		FOLLOWING_WALL,
    		TURNING_RIGHT,
    		TURNING_LEFT
    	};
      	
      	State state;
    public:
    
      RobotScript(int argc,char **argv);
      
      ~RobotScript(void);
      
      void robotScanner(const sensor_msgs::LaserScan& msg);

      float degrees_to_radians(float angle);
      
  };

#endif
