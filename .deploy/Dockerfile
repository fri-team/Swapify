FROM alpine

RUN apk add --no-cache openssh rsync sshpass

WORKDIR /ci
COPY ./*.sh ./

CMD [ "/bin/sh", "/ci/deploy.sh" ]
