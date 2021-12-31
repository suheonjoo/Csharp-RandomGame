using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{

    public partial class Form1 : Form
    {
        class CustomForm : Form {       //Form 클래스의 상속을 받는다
            public CustomForm()
            {
                
                MessageBox.Show("우리가 흔히 아는 묵찌빠게임을 위도우폼 기반으로 만들었습니다.");
                
            }
            
            private void Show(string v)
            {
                throw new NotImplementedException();
            }
        }
       
        







        //전역 변수 설정하기
        string[] states = {"가위,바위,보중 하나 선택하세요","비겼습니다.","공격권은 당신에게 있습니다. 공격하세요","공격권은 컴퓨터에게 있습니다. 수비하세요",
           "당신이 이겼습니다.","당신이 졌습니다."};

        //가위바위보 상태 설정
        //프로그래밍 상태 설정 기본적으로 숫자로 되어 있어서 enum자료형을 사용해서 
        //숫자에 명칭을 붙여주기 위해서 사용함
        enum State { CHOICE, EVEN, ATTACK, DEFENSE, WIN, LOSE };

        //현재 상태는 일단 choice로 설정하기
        State state = State.CHOICE;

        //랜덤 함수만들어서 컴퓨터의 상태 난수로 설정하기
        Random Rando = new Random();

        //컴퓨터가 내는 결과를 일단 1로 설정
        int comresult = 1;

        //유저가 낸 결과도 설정
        int myresult = 1;

        //벌어진 횟수 0으로 설정
        int nCount = 0;

        //기다리는 시간 설정
        int timeWait = 0;
        public Form1()
        {
            InitializeComponent();


            //무명 델리게이터 사용하여 이벤트와 연결시키기
            //이 버튼은 게임 설명 버튼
            Button button = new Button();

            button.Location = new Point(240,10);
            button.Size = new Size(109, 27);
            button.Text = "게임설명";
            button.Click += delegate (object sender, EventArgs args)
            {
                MessageBox.Show("우리가 흔히 아는 묵찌빠 게임입니다.\n1.우선 가위바위보를 하여 공격권을 정합니다.\n2.공격권을 가진 사람은 수비하는 사람과 같은 동작이 되도록 가위바위보를 합니다.(그때 같을시 공격자 WIN!!)\n**선택은 (사용자::)이 부분에서 이미지를 클릭하시면 됩니다.**\n**(컴퓨터::)는 컴퓨터의 결과부분입니다**");
            };
            button.Click += (sender, args) =>
            {
                MessageBox.Show("이제 게임을 시작하세요");
            };

            // 버튼을 화면에 추가합니다.
            Controls.Add(button);
        }
        //100분의 1초마다 한번씩 실행하게하기
        //폼에 들어가서 timer interval 10으로 설정하기

        private void timer1_Tick(object sender, EventArgs e)
        {
            comresult = Rando.Next(1, 4);  //컴퓨터가 낸 결과를 1부터 3까지중 하나 난수 발생하기

            //만약에 비긴 경우 timeWait만큼 시간을 기다린후 공격권 결정을 또 함
            if (state == State.EVEN)
            {    //비길경우
                if (timeWait > 0)
                {
                    timeWait -= timer1.Interval;    //기다리는 시간은 타이머 시간 간격 만큼 빼줌
                    return;
                }
                else
                {
                    state = State.CHOICE;
                    ViewMsg();      //해당 이미지 보여주기
                }
            }

            //비기지 않았을 때 이미지를 로테이션 해주기
            else if (state == State.CHOICE)
            {
                ChangeImage(ref comresult);     //컴퓨터 상태를 인자를 주어 이미지를 변하게 하기
            }
        }
        //내가 묵을 낼때 
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ResultCase(1);
        }
        //내가 가위를 낼때
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            ResultCase(2);
        }
        //내가 묵을 낼때
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            ResultCase(3);
        }
        private void ResultCase(int nMy) {
            int nStatus;        //이겼는지 졌는지 상태 변수
            //시간이 진행중일 경우 함수 나가기
            if (timeWait > 0) {
                return;
            }
            //이기거나 질 경우도 함수 나가기
            if (state == State.WIN || state == State.LOSE) {
                return;
            }
            //위와 같은 경우가 아니라면(묵찌빠 진행중) 게임 시행횟수를 늘리기 
            nCount++;
            myresult = nMy;//받은 함수인자를 유저가 낸결과라고 내가 따로 지정한 전역 변수에 넣음
            //시행 했을때 이겼는지 졌는지 판단하기

            //컴퓨터가 낸결과와 내가 낸결과 판단해서
            if (nMy > comresult)
            {
                nStatus = -1;
            }
            else if (nMy == comresult)      //이거는 비긴 상태
            {
                nStatus = 0;
            }
            else {
                nStatus = 1;
            }
            //찌가 아닐경우 즉 묵 또는 빠일 경우
            if (nMy != 2) {
                //빠 묵만 있는 상태에서 내가 이겼을 경우를 1로 졌을 경우는 -1로 하기
                if (nStatus == -1 && comresult == 1) { nStatus = 1; }
                if (nStatus == 1 && comresult == 3) { nStatus = -1; }
            }
            // 이후 공격권을 결정하기
            if (state == State.CHOICE)
            {
                switch (nStatus)
                {
                    case -1:
                        state = State.DEFENSE;    //내가 졌을때는 방어 상태로
                        break;
                    case 0:
                        state = State.EVEN;       //내가 비겼을 경우 기다리기
                        timeWait = 500;
                        break;
                    case 1:
                        state = State.ATTACK;     //내가 이겼을 경우 공격 상태로 지정하기
                        break;
                }
            }

            // 공격중일 때 상태 설정하기
            if (state == State.ATTACK)
            {
                ChangeImage(ref comresult);     //공격중일때의 이미지 바꾸기
                switch (nStatus)
                {
                    case -1:
                        state = State.DEFENSE;
                        break;
                    case 0:
                        state = State.WIN;
                        break;
                    case 1:
                        state = State.ATTACK;
                        break;
                }

            }
            //내가 방어할때
            if (state == State.DEFENSE)
            {
                ChangeImage(ref comresult);     //내가 방어 할때 이미지 바꾸기
                switch (nStatus)
                {
                    case -1:
                        state = State.DEFENSE;
                        break;
                    case 0:
                        state = State.LOSE;
                        break;
                    case 1:
                        state = State.ATTACK;
                        break;
                }
            }

            ViewMsg();      //내가 몇번 게임했는지 시행결과 보여주기
        }
        private void ViewMsg() {
            //내가 지금 어떤 상태인지 보여주는 라벨
            label1.Text = (nCount + 1) + "회:" + states[(int)state];
        }
        //이미지는 resource라는 파일 만들어서 이미를 넣었음
        private void ChangeImage(ref int nNumber) {
            switch (nNumber)
            {
                case 1:
                    pictureBox4.Image = global::WindowsFormsApplication3.Properties.Resources.rock;
                    break;
                case 2:
                    pictureBox4.Image = global::WindowsFormsApplication3.Properties.Resources.scissor;
                    break;
                case 3:
                    pictureBox4.Image = global::WindowsFormsApplication3.Properties.Resources.paper;
                    break;
            }
        }
        //우선 초기 상태 만들기
        private void Form1_Load(object sender, EventArgs e)
        {
            nCount = 0;
            state = State.CHOICE;
            ViewMsg();
            timer1.Start();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }
        //이거는 다시 초기화 해주는 버튼
        private void button1_Click(object sender, EventArgs e)
        {
            nCount = 0;
            state = State.CHOICE;
            ViewMsg();
        }
        //이거는 이 게임을 종료해주는 버튼
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        static void main() {
            CustomForm title = new CustomForm();
            Application.Run(new Form1());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CustomForm form = new CustomForm();
            //form.Show();
            //MessageBox.Show("우리가 흔히 아는 묵찌빠게임을 위도우폼 기반으로 만들었습니다.");
        }

       
    }
}
