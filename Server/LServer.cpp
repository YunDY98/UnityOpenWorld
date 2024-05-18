#define  _WINSOCK_DEPRECATED_NO_WARNINGS
//
#include <WinSock2.h>
#include <iostream>
#include <string>
#include <Windows.h>
#include <vector>
#include <process.h>
#include <memory>
#include <format>

///Users/user/source/repos/Server/mysql840debug/include/jdbc
#include "mysql_connection.h"
#include "cppconn/driver.h"
#include "cppconn/exception.h"
#include "cppconn/prepared_statement.h"
//
using namespace std;
#pragma comment (lib, "WS2_32.lib")
#pragma comment (lib, "mysqlcppconn.lib")
//
#define SERVER_IPV4 "0.0.0.0"
#define SERVER_PORT 8888
#define PACKET_SIZE 100
#define PACKET_ID 20
#define PACKET_PWD 20
#define ACTION_INFO 2
#define ASCII_INT 48


//

//
const string server = "tcp://127.0.0.1:3306";
const string username = "root";
const string password = "2341";
const string schema = "login";
//vector<SOCKET> userlist;
CRITICAL_SECTION ServerCS;

// sql 세팅
sql::Driver* driver = nullptr;
sql::Connection* con = nullptr;
sql::Statement* stmt = nullptr;
sql::PreparedStatement* pstmt = nullptr;
sql::ResultSet* rs = nullptr;

bool bProgramRunning = true;
bool bNetworkConnected = false;

enum State
{
	Login,
	LoginFail,
	Join,
	JoinFail,
	Connecting
};

unsigned WINAPI Chatting(void* arg)
{

	

	SOCKET ClientSocket = *(SOCKET*)arg;


	
	char RecvUserInfo[PACKET_SIZE] = { 0, };
	

	int RecvUserInfoBytes = 0;

	
	string _sAction, _sID, _sPWD;
	SOCKET LogoutSocket;
	//로그인
	while (true)
	{
	
		
		RecvUserInfoBytes = recv(ClientSocket, RecvUserInfo, sizeof(RecvUserInfo), 0);
		
		if (RecvUserInfoBytes > 0)
		{
			char _Action[ACTION_INFO];
			memcpy(_Action, RecvUserInfo, ACTION_INFO);
			char _ID[PACKET_ID];

			//if(_Action == 유저의 레벨 경험치 위치를 가져오는 경우)
			memcpy(_ID, RecvUserInfo + ACTION_INFO, PACKET_ID);
			char _PWD[PACKET_PWD];
			memcpy(_PWD, RecvUserInfo + ACTION_INFO + PACKET_ID, PACKET_PWD);
			_sAction = _Action, _sID = _ID, _sPWD = _PWD;
		}
		
		
		char _state;

	
		/*pstmt = con->prepareStatement("UPDATE Users SET Connecting = (?) WHERE ID = ?");
		pstmt->setString(2, _sID);
		pstmt->setBoolean(1, false);
		pstmt->execute();*/
		
		if (RecvUserInfoBytes <= 0)
		{
			//접속 종료 
			
			// 위치 , 경험치 , 레벨, 보유 아이템 이떄 업데이트  
			cout << "접속 종료" << endl;

			pstmt = con->prepareStatement("UPDATE Users SET Connecting = (?) WHERE ID = ?");
			pstmt->setString(2, _sID);
			pstmt->setBoolean(1, false);
			pstmt->execute();

			break;
			
		}
		
		

		if (RecvUserInfoBytes > 0)
		{
			
			if (_sAction == to_string(State::Login))
			{
				bool _checkLogin;

				pstmt = con->prepareStatement("SELECT Connecting from Users Where ID = ?");
				pstmt->setString(1, _sID);
				
				sql::ResultSet* RS = pstmt->executeQuery();
				if (RS->next())
				{
					_checkLogin = RS->getBoolean("Connecting");
					if (_checkLogin)
					{
						_state = State::Connecting + ASCII_INT;
						cout << "이미 접속중" << endl;
						send(ClientSocket, &_state, 1, 0);
						
						
					}
					else
					{
						pstmt = con->prepareStatement("select * from Users where ID = ? and PWD = sha2(?,256)");
						pstmt->setString(1, _sID);
						pstmt->setString(2, _sPWD);
						pstmt->execute();



						RS = pstmt->executeQuery();

						if (RS->rowsCount() > 0)
						{

							cout << "로그인 성공" << endl;
							_state = State::Login + ASCII_INT;


							pstmt = con->prepareStatement("UPDATE Users SET Connecting = (?) WHERE ID = ?");
							pstmt->setBoolean(1, true);
							pstmt->setString(2, _sID);
							pstmt->execute();
							send(ClientSocket, &_state, 1, 0);
						}
						else
						{
							_state = State::LoginFail + ASCII_INT;
							cout << "아이디 또는 비밀번호 틀림" << endl;
							send(ClientSocket, &_state, 1, 0);
						}
						RS->close();
					}



					
				}
				
				
				
			
				
			}

			if (_sAction == to_string(State::Join))
			{

				
				pstmt = con->prepareStatement("select * from Users where ID = ?");
				pstmt->setString(1, _sID);
				pstmt->execute();
				sql::ResultSet* RS = pstmt->executeQuery();
				if (RS->rowsCount() > 0)
				{
					_state = State::JoinFail + ASCII_INT;
					cout << "중복된 아이디입니다" << endl;
					send(ClientSocket, &_state, 1, 0);

				}
				else
				{
					_state = State::Join + ASCII_INT;
					
					pstmt = con->prepareStatement("insert into Users (ID,PWD) values(?,sha2(?,256))");
					pstmt->setString(1, _sID);
					pstmt->setString(2, _sPWD);
					pstmt->execute();
					send(ClientSocket, &_state, 1, 0);
					cout << "가입 되었습니다" << endl;

				}

				RS->close();
			}

			memset(RecvUserInfo, NULL, PACKET_SIZE);
			
		}


	}

	EnterCriticalSection(&ServerCS);
	//userlist.erase(find(userlist.begin(), userlist.end(), LogoutSocket));
	closesocket(ClientSocket);
	LeaveCriticalSection(&ServerCS);
	return bProgramRunning = true;
}

int main(int argc, char* argv[])
{
	InitializeCriticalSection(&ServerCS);

	// sql 연결
	driver = get_driver_instance();
	con = driver->connect(server, username, password);

	
	try {
		con = driver->connect(server, username, password);
		con->setSchema("login");

		stmt = con->createStatement();





		rs = stmt->executeQuery("SELECT * FROM Users");

		if (con) {
			cout << "데이터베이스 연결 성공!" << endl;
		}
		else {
			cout << "데이터베이스 연결 실패..." << endl;
		}
	}
	catch (sql::SQLException& e) {
		cout << "SQL 예외 발생: " << e.what() << endl;
		
	}



	while (rs->next()) {
		cout << "ID: " << rs->getInt("Num");
		cout << ", Username: " << rs->getString("ID");
		cout << ", Password: " << rs->getString("PWD") << endl;
	}







	
	cout << "데이터베이스 접속성공!" << endl;

	bool bProgramRunning = true;
	bool bNetworkConnected = false;
	// winsock 세팅
	WSAData WSAdata = { 0, };
	if (WSAStartup(MAKEWORD(2, 2), &WSAdata) != 0) { exit(-1); }
	SOCKET ServerSocket = socket(AF_INET, SOCK_STREAM, 0);
	if (ServerSocket == INVALID_SOCKET) { exit(-2); }
	SOCKADDR_IN ServerSockAddr = { 0, };
	ServerSockAddr.sin_family = AF_INET; // IPV4
	ServerSockAddr.sin_addr.S_un.S_addr = inet_addr(SERVER_IPV4);
	ServerSockAddr.sin_port = htons(SERVER_PORT);
	if (::bind(ServerSocket, (SOCKADDR*)&ServerSockAddr, sizeof(ServerSockAddr)) != 0) { exit(-3); };
	if (listen(ServerSocket, SOMAXCONN) == SOCKET_ERROR) { exit(-4); };

	SOCKET ClientSocket = NULL;

	while (bProgramRunning)
	{
		// 클라 접속 세팅, 대기
		SOCKADDR_IN ClientAddrIn = { 0, };
		int ClientLength = sizeof(ClientAddrIn);
		cout << "클라이언트 접속대기!" << endl;
		ClientSocket = accept(ServerSocket, (SOCKADDR*)&ClientAddrIn, &ClientLength);
		if (ClientSocket == INVALID_SOCKET)
		{
			closesocket(ClientSocket);
			continue;
		}
		else
		{
			// 클라 접속 완료
			cout << "클라이언트 접속완료!!!" << endl;

			cout << "connect : " << ClientSocket << endl;
			EnterCriticalSection(&ServerCS);
			//userlist.push_back(ClientSocket);
			LeaveCriticalSection(&ServerCS);
			HANDLE ThreadHandle = (HANDLE)_beginthreadex(nullptr, 0, Chatting, (void*)&ClientSocket, 0, nullptr);
		}
	}



	closesocket(ServerSocket);
	DeleteCriticalSection(&ServerCS);

	WSACleanup();
	return 0;
}