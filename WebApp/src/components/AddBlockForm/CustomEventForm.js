import React, { useEffect, useState } from "react";
import { padStart } from "lodash";
import { TextField, Button } from "@mui/material";
import { FormControl, InputLabel, MenuItem, Select } from "@material-ui/core";
import { SliderPicker } from "react-color";
import { ClipLoader } from "react-spinners";
// Redux
import { useSelector, useDispatch } from "react-redux";
import { createBlock } from "../../redux/slices/block";

const CustomEventForm = ({ course, user }) => {
  const dispatch = useDispatch();
  const { isLoading } = useSelector((state) => state.block);

  const [isDisabled, setIsDisabled] = useState(true);
  const [customEvent, setCustomEvent] = useState({
    id: null,
    day: course.day,
    startBlock: padStart(`${course.startBlock + 6 || "07"}:00`, 5, "0"),
    endBlock: "",
    courseName:
      "Management of computer systems projects, Universidad Politécnica de Valencia, ESP",
    eventPlace: "",
    type: "Event",

    courseId: "string",
    courseCode: "5899",
    courseShortcut: "",

    length: null,
    colorOfBlock: "#3F51B5",
  });

  useEffect(() => {
    if (customEvent.courseName !== "" && customEvent.length != null) {
      setIsDisabled(false);
    } else {
      setIsDisabled(true);
    }
  }, [customEvent.courseName, customEvent.length]);

  const handleChange = (e) => {
    setCustomEvent((customEvent) => ({
      ...customEvent,
      [e.target.name]: e.target.value,
    }));
  };

  const handleChangeComplete = (color) => {
    console.log(color);
    setCustomEvent((customEvent) => ({
      ...customEvent,
      colorOfBlock: color.hex,
    }));
  };

  const onSubmitHandler = async () => {
    console.log(customEvent);
    dispatch(
      createBlock({
        timetableBlock: {
          ...customEvent,
          endBlock: padStart(
            `${
              parseInt(customEvent.startBlock) + parseInt(customEvent.length) ||
              "07"
            }:00`,
            5,
            "0"
          ),
        },
        user: user,
      })
    );
  };

  return (
    <React.Fragment>
      <TextField
        placeholder="Zadajte názov udalosti *"
        name="courseName"
        value={customEvent.courseName}
        onChange={handleChange}
        margin="normal"
        fullWidth
        variant="standard"
        required
      />
      <TextField
        label="Miesto"
        placeholder="Miesto"
        name="eventPlace"
        value={customEvent.eventPlace}
        onChange={handleChange}
        margin="normal"
        variant="standard"
        fullWidth
      />
      <FormControl fullWidth required>
        <InputLabel>Deň</InputLabel>
        <Select
          name="day"
          value={customEvent.day}
          variant="standard"
          onChange={handleChange}
        >
          <MenuItem value={1}>Pondelok</MenuItem>
          <MenuItem value={2}>Utorok</MenuItem>
          <MenuItem value={3}>Streda</MenuItem>
          <MenuItem value={4}>Štvrtok</MenuItem>
          <MenuItem value={5}>Piatok</MenuItem>
        </Select>
      </FormControl>
      <TextField
        label="Začiatok"
        type="time"
        inputProps={{ step: 3600 }}
        name="startBlock"
        value={customEvent.startBlock}
        onChange={handleChange}
        margin="normal"
        fullWidth
        variant="standard"
        required
      />
      <TextField
        label="Dĺžka"
        type="number"
        InputProps={{
          inputProps: {
            min: 1,
            max: 20 - customEvent.startBlock.substring(0, 2),
          },
        }}
        name="length"
        value={customEvent.length}
        onChange={handleChange}
        margin="normal"
        fullWidth
        variant="standard"
        required
      />
      <TextField
        label="Farba bloku"
        type="text"
        inputProps={{ step: 3600 }}
        name="colorOfBlock"
        value={customEvent.colorOfBlock}
        onChange={handleChange}
        margin="normal"
        fullWidth
        variant="standard"
        required
      />
      <SliderPicker
        color={customEvent.colorOfBlock}
        onChangeComplete={handleChangeComplete}
      />
      {isLoading == false ? (
        <Button
          disabled={isDisabled}
          onClick={onSubmitHandler}
          color="primary"
          variant="contained"
          sx={{ alignSelf: "flex-end", marginTop: "10px" }}
        >
          Uložiť
        </Button>
      ) : (
        <ClipLoader size={35} color={"#2196f3"} loading={isLoading} />
      )}
    </React.Fragment>
  );
};

export default CustomEventForm;
