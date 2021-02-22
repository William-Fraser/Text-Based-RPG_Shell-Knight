﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    abstract class GameCharacter
    {
        protected Toolkit toolkit = new Toolkit();

        protected static int DIRECTION_UP = 0;
        protected static int DIRECTION_RIGHT = 1;
        protected static int DIRECTION_DOWN = 2;
        protected static int DIRECTION_LEFT = 3;
        protected private int _directionMoving;

        protected int _posX;
        protected int _posY;

        protected string _name;
        protected char _avatar;
        protected private bool _alive = true;
        protected private int[] _health = new int[2]; // set: current health / max health
        protected private int[] _damage = new int[2]; // set range: Highest / Lowest
        
        // ----- gets/sets
        public int getX()
        {
            return _posX;
        }
        public int getY()
        {
            return _posY;
        }
        public int getDirection() { 
            return _directionMoving;
        }
        public bool getAlive() 
        {
            return _alive;
        }

        // ----- private sets
        private int setToLimits(int currentStatus, int maxStatus)
        {
            int fixedStatus = currentStatus;
            if (currentStatus <= 0)
            {
                fixedStatus = 0;
            }else if (currentStatus >= maxStatus)
            {
                fixedStatus = maxStatus;
            }return fixedStatus;
        }
        
        // ----- private methods
        private void moveBack() // moves player back if they're supposed to collide with something
        {
            if (_directionMoving == DIRECTION_UP) { Move(DIRECTION_DOWN); }
            else if (_directionMoving == DIRECTION_DOWN) { Move(DIRECTION_UP); }
            else if (_directionMoving == DIRECTION_LEFT) { Move(DIRECTION_RIGHT); }
            else if (_directionMoving == DIRECTION_RIGHT) { Move(DIRECTION_LEFT); }
        }
        private void killCharacter()
        {
            if (_health[1] <= 0) {
                string deathMessage = $"< {_name} has been slain >";
                _alive = false;
                toolkit.DisplayText(deathMessage);
                Console.ReadKey(true);
            }
        }
        private void takeDamage(int[] damage) 
        {
            int finalDamage = toolkit.RandomNumBetween(damage[1], damage[0]);
            string damageMessage = $"< {_name} taking {finalDamage} points of Damage >";
            _health[1] -= finalDamage;
            toolkit.DisplayText(damageMessage);
            setToLimits(_health[0],_health[1]);
            moveBack();
        }

        // ----- public methods
        public void Move(int DIRECTION_) //moves the player in the specifyed DICRECTION_ 
        {
            if (DIRECTION_ == DIRECTION_DOWN) { _posY += 1; _directionMoving = DIRECTION_DOWN; }
            else if (DIRECTION_ == DIRECTION_UP) { _posY -= 1; _directionMoving = DIRECTION_UP; }
            else if (DIRECTION_ == DIRECTION_LEFT) { _posX -= 1; _directionMoving = DIRECTION_LEFT; }
            else if (DIRECTION_ == DIRECTION_RIGHT) { _posX += 1; _directionMoving = DIRECTION_RIGHT; }
        }
        public void Draw()
        {
            if (_alive)
            {
                //Console.SetCursorPosition(1, 1);              //
                //Console.Write($"X: { _posX } Y: { _posY }");  // --- debug / display cursor XY
                Console.SetCursorPosition(_posX, _posY);
                Console.Write(_avatar);
            }
        }
        public void CheckForWall(char map, string walls)
        {
            char[] wallGroup = new char[walls.Length];
            for (int i = 0; i < walls.Length; i++) {
                wallGroup[i] = walls[i];
            }
            for (int i = 0; i < wallGroup.Length; i++) {
                if (map == wallGroup[i]) {
                    moveBack();
                }
            }
        }
        public void ChecktoTakeDamage(bool attack, int[] damage) // change to start battle // if bool true attack success 
        {
            if (_alive)
            {
                if (attack)
                {
                        takeDamage(damage);
                        killCharacter();
                }
                else { }
            }
        }
        public bool Attack(int enemyX, int enemyY)
        {
            if (_alive)
            {
                if (enemyX == _posX)
                {
                    if (enemyY == _posY)
                    {
                        moveBack();
                        return true;
                    }
                }
            }
            return false;
        }
    }

}
