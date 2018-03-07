#!/bin/sh

echo "Copying install script to remote host..."
sshpass -p $SSH_PASS scp -q -o LogLevel=QUIET -o StrictHostKeyChecking=no /ci/install.sh $SSH_USER@$SSH_HOST:/home/$SSH_USER
echo "Starting install script on remote host..."
sshpass -p $SSH_PASS ssh $SSH_USER@$SSH_HOST '~/install.sh'
