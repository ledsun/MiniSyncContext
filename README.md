# MiniSyncContext
C# �� async/await �̗������u�C�x���g���[�v�v�ƁuSynchronizationContext�v�����삵�Ċώ@����f���v���W�F�N�g�B

�uawait �̓����ŉ����N���Ă��邩�H�v�𗝉����邽�߂̊w�K�p�����ł��B

## ���s���@

```bash
git clone https://github.com/ledsun/MiniSyncContext.git
cd MiniSyncContext
dotnet run
```

## ���ʗ�

```
[tid:05] PeriodicEvents started.
[tid:11] I'm main
[tid:10] I'm sub
[tid:11] Ctx=MySyncContext
[tid:11] await 500ms
[tid:11] fibonacci 2
[tid:11] fibonacci 3
[tid:11] fibonacci 5
[tid:11] end await. await sub thread...
[tid:11] fibonacci 8
[tid:11] fibonacci 13
[tid:10] bye!
[tid:11] bye!
Done.
```