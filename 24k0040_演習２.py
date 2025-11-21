import time

# nQueen のサイズ
n = 8

# 全解探索の時には初期値を0にして、解の数を計算
# -1の時は、単独解の探索
count = -1

# スタートメソッド
def start():
    startTime = time.process_time()
    
    # 初期化
    board = init()

    # 探索開始
    search()

    # 複数解探索の場合は、見つかった解の数を表示
    if count >= 0:
        print(count)
        
    endTime = time.process_time()
    print(endTime - startTime)

# 初期化
def init():
    # 盤面の配列を作る
    return [-1] * n

# 探索
# @param board 盤面
# @param depth 探索の深さ
# @return 探索が成功した場合 True、失敗した場合 False

def search():
    global count
    stack = []                     
    stack.append(([-1] * n, 0))    

    while stack:
        board, depth = stack.pop()  

        for i in range(n):
            new_board = board.copy()
            new_board[depth] = i

            if check(new_board, depth):
                if depth + 1 == n:
                    printBoard(new_board)
                    if count >= 0:
                        count += 1
                    else:
                        return True
                else:
                    stack.append((new_board, depth + 1))
    return False


###ここを完成させる


# 中間状態の評価
# @param board 盤面
# @param depth 探索の深さ
# @return 盤面に矛盾がないとき true、矛盾があるとき false
def check(board, depth):
    for i in range(depth):
        # 縦チェック
        if board[depth] == board[i]:
            return False

        # 斜め左上チェック
        if board[depth] - (depth - i) == board[i]:
            return False

        # 斜め右上チェック
        if board[depth] + (depth - i) == board[i]:
            return False
    return True

# 結果のコンソール出力
# @param board
def printBoard(board):
    #print("")
    for i in range(n):
        line = ""
        for j in range(n):
            if board[i] == j:
                line += "*"
            else:
                line += "."
        #print(line)

# 実行
start()
