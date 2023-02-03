import React, { PureComponent } from "react";
import { textTransform } from "text-transform";
import PropTypes from "prop-types";
import onClickOutside from "react-onclickoutside";
import _, { padStart, parseInt, replace } from "lodash";
import toMaterialStyle from "material-color-hash";
import Tooltip from "@material-ui/core/Tooltip";
import Zoom from "@material-ui/core/Zoom";
import IconButton from "@material-ui/core/IconButton";
import DeleteIcon from "@material-ui/icons/Delete";
import EditIcon from "@material-ui/icons/Edit";
import ClearIcon from "@material-ui/icons/Clear";
import AddIcon from "@material-ui/icons/Add";
import SwapIcon from "@material-ui/icons/SwapHorizSharp";
import AddBlockForm from "../AddBlockForm/AddBlockForm";
//import CustomEventForm from '../AddBlockForm/CustomEventForm';
import Location from "../svg/Location";
import Person from "../svg/Person";
import "./BlockDetail.scss";
//import { Tab } from "@material-ui/core";
//import { userInfo } from "os";

class BlockDetail extends PureComponent {
  state = {
    dialogOpen: false,
    course: { ...this.props.course },
  };

  textTrans = (text) => {
    let a = textTransform(text, "title");
    return a;
  };

  calculateBottomPosition = (pTop) => {
    if (pTop > window.innerHeight * 0.7) return `2px`;
    else return `auto`;
  };

  calculateTopPosition = (pTop) => {
    if (pTop > window.innerHeight * 0.7) return `auto`;
    else return `${pTop}px`;
  };

  handleClickOutside = () => {
    if (!this.state.dialogOpen) {
      this.props.onOutsideClick();
    }
  };

  handleClickExchange = () => this.props.onExchangeRequest(this.props.course);

  handleClickDelete = () => {
    this.props.onClickDelete(this.props.course);
    this.setState({ dialogOpen: false });
  };

  handleClickEdit = () => {
    this.props.onClickEdit();
    this.setState({ dialogOpen: false });
  };

  handleClickAdd = () => {
    const body = {
      user: this.props.user,
      timetableBlock: {
        id: this.props.course.id,
        day: this.props.course.day,
        courseCode: this.props.course.courseCode,
        startBlock:
          parseInt(
            replace(
              padStart(
                `${this.props.course.startBlock + 6 || "07"}:00`,
                5,
                "0"
              ),
              /[^0-9]/,
              ""
            )
          ) / 100,
        endBlock:
          parseInt(
            replace(
              padStart(
                `${this.props.course.startBlock + 6 || "07"}:00`,
                5,
                "0"
              ),
              /[^0-9]/,
              ""
            )
          ) /
            100 +
          parseInt(this.props.course.endBlock - this.props.course.startBlock),
        courseName: this.props.course.courseName,
        courseShortcut: this.props.course.courseShortcut,
        room: this.props.course.room,
        teacher: this.props.course.teacher,
        type: this.props.course.type,
      },
    };

    this.props.onClickAdd(body);
  };

  onClickEditBlock = () => {
    //ak je to typ bloku event musi sa dialogOpen : true ale s tabValue = 1
    this.setState({ dialogOpen: true  });
  };

  onCloseEditBlock = () => {
    this.setState({ dialogOpen: false });
  };

  showEditButton = (color) => {
    if (this.props.course.isMine) {
      return (
        <Tooltip
          title="Upraviť blok"
          placement="top"
          TransitionComponent={Zoom}
        >
          <IconButton onClick={this.onClickEditBlock}>
            <EditIcon nativecolor={color} />
          </IconButton>
        </Tooltip>
      );
    }
  };

  showExchangeButton = (color) => {
    if (this.props.course.isMine) {
      return (
        <Tooltip
          title="Požiadať o výmenu"
          placement="top"
          TransitionComponent={Zoom}
        >
          <IconButton onClick={this.handleClickExchange}>
            <SwapIcon nativecolor={color} />
          </IconButton>
        </Tooltip>
      );
    }
  };

  showAddButton = (color) => {
    if (!this.props.course.isMine) {
      return (
        <Tooltip title="Pridať blok" placement="top" TransitionComponent={Zoom}>
          <IconButton onClick={this.handleClickAdd}>
            <AddIcon nativecolor={color} />
          </IconButton>
        </Tooltip>
      );
    }
  };

  convertNameToEmail(teacherName) {
    let result = "";
    // remove titles from name
    let charsBeforeDot = 0;
    for (let i = 0; i < teacherName.length; i++) {
      result += teacherName[i];
      charsBeforeDot++;
      if (teacherName[i] === ".") {
        result = result.substring(0, result.length - charsBeforeDot);
      }
      if (teacherName[i] === " ") {
        charsBeforeDot = 0;
      }
    }

    result = result.trim();
    if (result !== "") {
      result =
        _.replace(_.lowerCase(_.deburr(result)), " ", ".") + "@fri.uniza.sk";
    } else {
      result = null;
    }

    return result;
  }

  setBlockColor(shade) {
    return {
      backgroundColor: shade,
      color: "rgba(0, 0, 0, 0.87)",
    };
  }

  render() {
    if (!this.props.isVisible) {
      return null;
    }
    const { top, left, course, user } = this.props;
    const email = this.convertNameToEmail(course.teacher);
    //const UsersName = this.user;
    const place = course.room;
    const room = course.room !== "" ? ", " + course.room : "";
    const { backgroundColor, color } =
      course.blockColor == null
        ? toMaterialStyle(course.courseCode || "", course.blockColor)
        : this.setBlockColor(course.blockColor);
    const style = {
      top: `${this.calculateTopPosition(top)}`,
      left: `${left}px`,
      bottom: `${this.calculateBottomPosition(top)}`,
      position: `absolute`,
      width: `20%`,
    };
    const dialogOpen = this.state.dialogOpen;
    return (
      <div className="block-detail" style={style}>
        <div className="header" style={{ backgroundColor }}>
          <div className="buttons">
            {
              <span>
                {course.type !== "event" && this.showEditButton(color)}
                {course.type !== "lecture" &&
                  course.type !== "event" &&
                  this.showExchangeButton(color)}
                {this.showAddButton(color)}
                <Tooltip
                  title="Vymazať blok"
                  placement="top"
                  TransitionComponent={Zoom}
                >
                  <IconButton onClick={this.handleClickDelete}>
                    <DeleteIcon nativecolor={color} />
                  </IconButton>
                </Tooltip>
              </span>
            }
            <IconButton onClick={this.handleClickOutside}>
              <ClearIcon nativecolor={color} />
            </IconButton>
          </div>
          <div className="name" style={{ color }}>
            {this.textTrans(course.courseName)}
          </div>
        </div>
        <div className="footer">
          {/*
          {course.type !== 'lecture' && (
            <div className="line">
              <SwapHorizontal className="icon" />
              <div className="text">
                <div className="medium">Vymieňam za Pondelok 13:00</div>
              </div>
            </div>
          )}
          */}
          <div className="line">
            <Location className="icon" />
            <div className="text">
              <div className="medium">
                {course.type == "event" ? `${place}` : `FRI${room}`}
              </div>
              {/*
              <div className="small">kapacita 20</div>
              */}
            </div>
          </div>
          <div className="line">
            <Person className="icon" />
            <div className="text">
              <div className="medium">
                {course.type == "event" ? user.email : course.teacher}
              </div>
              <div className="small">
                {course.type == "event" ? " " : email}
              </div>
            </div>
          </div>
        </div>
        {dialogOpen && (
          <AddBlockForm
            id={this.props.course.id}
            user={user}
            course={course}
            onSubmitClick={this.handleClickEdit}
            onCloseEditBlock={this.onCloseEditBlock}
            onClose={this.handleClickOutside}
            editing={true}
          />
        )}
      </div>
    );
  }
}

BlockDetail.propTypes = {
  isVisible: PropTypes.bool.isRequired,
  top: PropTypes.number.isRequired,
  left: PropTypes.number.isRequired,
  course: PropTypes.shape({}).isRequired,
  onOutsideClick: PropTypes.func.isRequired,
  onExchangeRequest: PropTypes.func.isRequired,
};

export default onClickOutside(BlockDetail);
