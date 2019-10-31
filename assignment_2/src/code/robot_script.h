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
#include "std_msgs/String.h"

class RobotScript
  {
    private:
      
    public:
    
      RobotScript(int argc,char **argv);
      ~RobotScript();
      Subscriber(const std_msgs::String::ConstPtr& msg);
  };