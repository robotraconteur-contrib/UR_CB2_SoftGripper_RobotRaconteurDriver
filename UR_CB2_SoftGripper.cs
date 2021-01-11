using System;
using System.Collections.Generic;
using System.Text;
using com.robotraconteur.robotics.tool;
using com.robotraconteur.robotics.robot;
using RobotRaconteur;
using System.Threading;

namespace UR_CB2_SoftGripper_RobotRaconteurDriver
{
    class UR_CB2_SoftGripper : Tool_default_impl, IDisposable
    {
        
        protected ServiceSubscription _robot_sub;

        public UR_CB2_SoftGripper(string robot_url)
        {
            _robot_sub = RobotRaconteurNode.s.SubscribeService(robot_url);
        }

        public override void close()
        {
            
            Robot robot = (Robot)_robot_sub.GetDefaultClient();
            robot.setf_signal("DO5", new double[] { 1.0 });
            robot.setf_signal("DO4", new double[] { 0 });
            robot.setf_signal("DO3", new double[] { 1.0 });
            while (robot.getf_signal("AI0")[0] < 3.6)
            {
                Thread.Sleep(10);
            }
            robot.setf_signal("DO5", new double[] { 0.0 });
            robot.setf_signal("DO3", new double[] { 0.0 });
        }

        public override void open()
        {
            Robot robot = (Robot)_robot_sub.GetDefaultClient();
            robot.setf_signal("DO5", new double[] { 1.0 });
            robot.setf_signal("DO4", new double[] { 1.0 });
            while (robot.getf_signal("AI0")[0] > 3.1)
            {
                Thread.Sleep(10);
            }
            robot.setf_signal("DO5", new double[] { 0.0 });
            robot.setf_signal("DO4", new double[] { 0.0 });
        }

        public void Dispose()
        {
            _robot_sub?.Close();
        }
    }
}
