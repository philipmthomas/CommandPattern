using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Moq;
using System.Linq;

namespace Tests
{
    public class Tests
    {
        private MockRepository mockRepository;

        [SetUp]
        public void Setup()
        {
            mockRepository = new MockRepository(MockBehavior.Strict);
        }

        [Test]
        public void MoveForwardCommandTest()
        {
            #region ARRANGE
            var mockRoverWrapper = mockRepository.Create<Rover>();
            var rover = mockRoverWrapper.Object;

            mockRoverWrapper.Setup(x => x.MoveForward());
            mockRoverWrapper.SetupProperty(x => x.RoverState, new ForwardState(rover));

            var command = new MoveForwardCommand();
            #endregion

            #region ACT
            command.Execute(rover);
            #endregion

            #region ASSERT
            mockRoverWrapper.Verify(x => x.MoveForward(), Times.Exactly(1));

            Assert.IsInstanceOf<ForwardState>(rover.RoverState);
            #endregion
        }

        [Test]
        public void MoveTurnLeftCommandTest()
        {
            #region ARRANGE
            var mockRoverWrapper = mockRepository.Create<Rover>();
            var rover = mockRoverWrapper.Object;

            mockRoverWrapper.Setup(x => x.MoveTurnLeft());
            mockRoverWrapper.SetupProperty(x => x.RoverState, new TurnLeftState(rover));

            var command = new MoveTurnLeftCommand();
            #endregion

            #region ACT
            command.Execute(rover);
            #endregion

            #region ASSERT
            mockRoverWrapper.Verify(x => x.MoveTurnLeft(), Times.Exactly(1));

            Assert.IsInstanceOf<TurnLeftState>(rover.RoverState);
            #endregion
        }

        [Test]
        public void MoveTurnRightCommandTest()
        {
            #region ARRANGE
            var mockRoverWrapper = mockRepository.Create<Rover>();
            var rover = mockRoverWrapper.Object;

            mockRoverWrapper.Setup(x => x.MoveTurnRight());
            mockRoverWrapper.SetupProperty(x => x.RoverState, new TurnRightState(rover));

            var command = new MoveTurnRightCommand();
            #endregion

            #region ACT
            command.Execute(rover);
            #endregion

            #region ASSERT
            mockRoverWrapper.Verify(x => x.MoveTurnRight(), Times.Exactly(1));

            Assert.IsInstanceOf<TurnRightState>(rover.RoverState);
            #endregion
        }
    }

    public class Rover
    {
        public virtual RoverState RoverState { get; set; }
        public void Run(IList<IMoveCommand> Commands)
        {
            foreach (var command in Commands)
                command.Execute(this);
        }

        public void Move()
        {
            if (RoverState != default(RoverState))
                RoverState.Move();
        }
        public virtual void MoveForward() => RoverState = new ForwardState(this);
        public virtual void MoveTurnLeft() => RoverState = new TurnLeftState(this);
        public virtual void MoveTurnRight() => RoverState = new TurnRightState(this);
    }

    #region State
    public abstract class RoverState
    {
        public RoverState(Rover rover)
        {
            Rover = rover;
        }

        public Rover Rover { get; set; }
        
        public abstract void Move();
    }

    public class TurnLeftState : RoverState
    {
        public TurnLeftState(Rover rover) : base(rover)
        {
        }

        public override void Move()
        {
            Console.WriteLine("Move Turn Left");
        }
    }

    public class TurnRightState : RoverState
    {
        public TurnRightState(Rover rover) : base(rover)
        {
        }

        public override void Move()
        {
            Console.WriteLine("Move Turn Right");
        }
    }

    public class ForwardState : RoverState
    {
        public ForwardState(Rover rover) : base(rover)
        {
        }

        public override void Move()
        {
            Console.WriteLine("Move Forward");
        }
    }
    #endregion

    #region Command
    public interface IMoveCommand
    {
        void Execute(Rover rover);
    }

    public class MoveForwardCommand : IMoveCommand
    {
        public void Execute(Rover rover)
        {
            rover.MoveForward();
            rover.Move();
        }
    }

    public class MoveTurnLeftCommand : IMoveCommand
    {
        public void Execute(Rover rover)
        {
            rover.MoveTurnLeft();
            rover.Move();
        }
    }

    public class MoveTurnRightCommand : IMoveCommand
    {
        public void Execute(Rover rover)
        {
            rover.MoveTurnRight();
            rover.Move();
        }
    }

    #endregion
}