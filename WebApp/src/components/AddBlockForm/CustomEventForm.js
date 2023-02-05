import React, { useEffect, useState } from "react";
import { padStart, replace } from "lodash";
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
    eventPlace: "",
    type: "Event",
    courseShortcut: "",
    courseId: "string",
    courseCode: Math.random() * 1000,
    length: null,
    BlockColor: "#3F51B5",
    teacher: "Marek Tavač",
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
      BlockColor: color.hex,
    }));
  };

  const onSubmitHandler = async () => {
    dispatch(
      createBlock({
        timetableBlock: {
          ...customEvent,
          start: parseInt(replace(customEvent.startBlock, /[^0-9]/, "")) / 100,
          startBlock: parseInt(customEvent.startBlock.split(":")[0]),
          endBlock: parseInt(
            padStart(
              `${
                parseInt(customEvent.startBlock) +
                  parseInt(customEvent.length) || "07"
              }:00`,
              5,
              "0"
            )
          ),
          //length: parseInt(customEvent.length),
          length: (parseInt(customEvent.endBlock) - parseInt(customEvent.startBlock)),
          courseShortcut: customEvent.courseName,
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
        name="room"
        value={customEvent.room}
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
        name="BlockColor"
        value={customEvent.BlockColor}
        onChange={handleChange}
        margin="normal"
        fullWidth
        variant="standard"
        required
      />
      <SliderPicker
        color={customEvent.BlockColor}
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

console.log(CustomEventForm);
export default CustomEventForm;
